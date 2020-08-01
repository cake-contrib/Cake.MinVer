//
// Run the MinVer dotnet tool using the settings returned by a configurator
// and set the minimum major and minor version
//

#addin "nuget:?package=Cake.MinVer&version=0.1.0"

var version = MinVer(settings => settings.WithMinimumMajorMinor("2.5"));

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
