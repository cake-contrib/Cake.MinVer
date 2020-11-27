namespace Cake.MinVer
{
    internal sealed class MinVerEnvironmentVariables
    {
        // ReSharper disable InconsistentNaming

        // Environment variables that translate to MinVer arguments
        internal static readonly string MINVERAUTOINCREMENT = $"MINVER{nameof(MinVerSettings.AutoIncrement).ToUpperInvariant()}";
        internal static readonly string MINVERBUILDMETADATA = $"MINVER{nameof(MinVerSettings.BuildMetadata).ToUpperInvariant()}";
        internal static readonly string MINVERDEFAULTPRERELEASEPHASE = $"MINVER{nameof(MinVerSettings.DefaultPreReleasePhase).ToUpperInvariant()}";
        internal static readonly string MINVERMINIMUMMAJORMINOR = $"MINVER{nameof(MinVerSettings.MinimumMajorMinor).ToUpperInvariant()}";
        internal static readonly string MINVERTAGPREFIX = $"MINVER{nameof(MinVerSettings.TagPrefix).ToUpperInvariant()}";
        internal static readonly string MINVERVERBOSITY = $"MINVER{nameof(MinVerSettings.Verbosity).ToUpperInvariant()}";

        // Environment variables that change Cake.MinVer behavior
        internal static readonly string MINVERPREFERGLOBALTOOL = $"MINVER{nameof(MinVerSettings.PreferGlobalTool).ToUpperInvariant()}";
        internal static readonly string MINVERNOFALLBACK = $"MINVER{nameof(MinVerSettings.NoFallback).ToUpperInvariant()}";
        internal static readonly string MINVERTOOLPATH = $"MINVER{nameof(MinVerSettings.ToolPath).ToUpperInvariant()}";

        // ReSharper restore InconsistentNaming
    }
}
