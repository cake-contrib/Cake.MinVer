//
// Run the MinVer dotnet tool using specified settings
// and set the verbosity
//

#addin "nuget:?package=Cake.MinVer&version=0.1.0"

var settings = new MinVerSettings()
{
    Verbosity = MinVerVerbosity.Trace,
};

var version = MinVer(settings);

Task("Example")
    .Does(context =>
{
    context.Information($"Version: {version.Version}");
    context.Information($"Major: {version.Major}");
    context.Information($"Minor: {version.Minor}");
    context.Information($"Patch: {version.Patch}");
    context.Information($"PreRelease: {version.PreRelease}");
    context.Information($"BuildMetadata: {version.BuildMetadata}");
    context.Information($"AssemblyVersion: {version.AssemblyVersion}");
    context.Information($"FileVersion: {version.FileVersion}");
    context.Information($"InformationalVersion: {version.InformationalVersion}");
    context.Information($"PackageVersion: {version.PackageVersion}");
});

RunTarget("Example");
