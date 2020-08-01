var target       = Argument<string>("target", "pack");
var buildVersion = Argument<string>("buildVersion", "0.0.1-local");

#tool "nuget:?package=NuGet.CommandLine&version=5.6.0"

Task("clean")
    .Does(() =>
{
    CleanDirectory("./build/artifacts");
});

Task("restore")
    .IsDependentOn("clean")
    .Does(() =>
{
    NuGetRestore("./Cake.MinVer.sln");
});

Task("build")
    .IsDependentOn("clean")
    .Does(() =>
{
    DotNetCoreBuild("./Cake.MinVer.sln", new DotNetCoreBuildSettings
    {
        Configuration = "Debug",
        NoRestore = true,
        NoIncremental = false,
        ArgumentCustomization = args =>
            args.AppendQuoted($"-p:Version={buildVersion}")
                .AppendQuoted($"-p:ContinuousIntegrationBuild=true")
    });

    DotNetCoreBuild("./Cake.MinVer.sln", new DotNetCoreBuildSettings
    {
        Configuration = "Release",
        NoRestore = true,
        NoIncremental = false,
        ArgumentCustomization = args =>
            args.AppendQuoted($"-p:Version={buildVersion}")
                .AppendQuoted($"-p:ContinuousIntegrationBuild=true")
    });
});

Task("test")
    .IsDependentOn("build")
    .Does(() =>
{
    var settings = new DotNetCoreTestSettings
    {
        Configuration = "Release",
        NoRestore = true,
        NoBuild = true,
    };

    var projectFiles = GetFiles("./test/**/*.csproj");
    foreach (var file in projectFiles)
    {
        DotNetCoreTest(file.FullPath, settings);
    }
});

Task("pack")
    .IsDependentOn("test")
    .Does(() =>
{
    var releaseNotes = $"https://github.com/augustoproiete/Cake.MinVer/releases/tag/v{buildVersion}";

    DotNetCorePack("./src/Cake.MinVer/Cake.MinVer.csproj", new DotNetCorePackSettings
    {
        Configuration = "Release",
        NoRestore = true,
        NoBuild = true,
        OutputDirectory = "./build/artifacts",
        ArgumentCustomization = args =>
            args.AppendQuoted($"-p:Version={buildVersion}")
                .AppendQuoted($"-p:PackageReleaseNotes={releaseNotes}")
    });
});

Task("publish")
    .IsDependentOn("pack")
    .Does(context =>
{
    var url =  context.EnvironmentVariable("NUGET_URL");
    if (string.IsNullOrWhiteSpace(url))
    {
        context.Information("No NuGet URL specified. Skipping publishing of NuGet packages");
        return;
    }

    var apiKey =  context.EnvironmentVariable("NUGET_API_KEY");
    if (string.IsNullOrWhiteSpace(apiKey))
    {
        context.Information("No NuGet API key specified. Skipping publishing of NuGet packages");
        return;
    }

    foreach (var nugetPackageFile in GetFiles("./build/artifacts/*.nupkg"))
    {
        context.Information($"Publishing {nugetPackageFile}...");
        context.NuGetPush(nugetPackageFile, new NuGetPushSettings
        {
            Source = url,
            ApiKey = apiKey,
        });
    }
});

RunTarget(target);
