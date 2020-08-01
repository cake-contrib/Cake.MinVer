using Cake.Common.Tools.DotNetCore;
using Cake.Core.IO;

namespace Cake.MinVer
{
    /// <summary>
    /// Contains settings used by <see cref="MinVerTool" />.
    /// </summary>
    public class MinVerSettings : DotNetCoreSettings
    {
        /// <summary>
        /// Set the version part to be automatically incremented.
        /// --auto-increment &lt;VERSION_PART&gt;
        /// major, minor, or patch (default)
        /// </summary>
        public MinVerAutoIncrement AutoIncrement { get; set; }

        /// <summary>
        /// Set the build metadata.
        /// --build-metadata &lt;BUILD_METADATA&gt;
        /// </summary>
        public string BuildMetadata { get; set; }

        /// <summary>
        /// Set the default pre-release phase.
        /// --default-pre-release-phase &lt;PHASE&gt;
        /// alpha (default), preview, etc.
        /// </summary>
        public string DefaultPreReleasePhase { get; set; }

        /// <summary>
        /// Set the minimum major and minor version.
        /// --minimum-major-minor &lt;MINIMUM_MAJOR_MINOR&gt;
        /// 1.0, 1.1, 2.0, etc.
        /// </summary>
        public string MinimumMajorMinor { get; set; }

        /// <summary>
        /// Set the working directory for MinVer to use.
        /// --repo &lt;REPO&gt;
        /// </summary>
        public DirectoryPath Repo { get; set; }

        /// <summary>
        /// Set the tag prefix.
        /// --tag-prefix &lt;TAG_PREFIX&gt;
        /// </summary>
        public string TagPrefix { get; set; }

        /// <summary>
        /// Set the verbosity.
        /// --verbosity &lt;VERBOSITY&gt;
        /// error, warn, info (default), debug, or trace
        /// </summary>
        public new MinVerVerbosity Verbosity { get; set; }

        internal DotNetCoreVerbosity? ToolVerbosity => base.Verbosity;
    }
}
