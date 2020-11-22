namespace Cake.MinVer.Tests.Support
{
    internal static class MinVerToolOutputs
    {
        internal static readonly string[] OutputWhenNotAGitRepo =
        {
            "MinVer: warning : '.' is not a valid working directory. Using default version 0.0.0-alpha.0.",
            "MinVer: Calculated version 0.0.0-alpha.0.",
            "0.0.0-alpha.0",
        };

        internal static readonly string[] OutputWhenTagNotFound =
        {
            "MinVer: No commit found with a valid SemVer 2.0 version. Using default version 0.0.0-alpha.0.",
            "MinVer: Using { Commit: d34db33, Tag: null, Version: 0.0.0-alpha.0, Height: 42 }.",
            "MinVer: Calculated version 0.0.0-alpha.0.42.",
            "0.0.0-alpha.0.42",
        };

        internal static readonly string[] OutputWhenTagFoundDefaultVerbosity =
        {
            "MinVer: Using { Commit: d34db33, Tag: 'v5.0.0', Version: 5.0.0, Height: 8 }.",
            "MinVer: Calculated version 5.0.1-alpha.0.8.",
            "5.0.1-alpha.0.8",
        };

        internal static readonly string[] OutputWhenTagFoundVerbosityError =
        {
            "1.2.3-preview.0.4",
        };
    }
}
