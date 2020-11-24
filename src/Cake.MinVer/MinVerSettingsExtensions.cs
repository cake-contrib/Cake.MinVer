using System;
using System.ComponentModel;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.MinVer
{
    /// <summary>
    /// Extensions for <see cref="MinVerSettings" />.
    /// </summary>
    public static class MinVerSettingsExtensions
    {
        /// <summary>
        /// Set the version part to be automatically incremented.
        /// --auto-increment &lt;VERSION_PART&gt;
        /// major, minor, or patch (default)
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="autoIncrement">The version part to be automatically incremented.</param>
        /// <returns>The <paramref name="settings" /> instance with <see cref="MinVerSettings.AutoIncrement" /> set to <paramref name="autoIncrement" />.</returns>
        public static MinVerSettings WithAutoIncrement(this MinVerSettings settings, MinVerAutoIncrement autoIncrement)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (!Enum.IsDefined(typeof(MinVerAutoIncrement), autoIncrement))
            {
                throw new InvalidEnumArgumentException(nameof(autoIncrement), (int)autoIncrement,
                    typeof(MinVerAutoIncrement));
            }

            settings.AutoIncrement = autoIncrement;

            return settings;
        }

        /// <summary>
        /// Set the build metadata.
        /// --build-metadata &lt;BUILD_METADATA&gt;
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="buildMetadata">The build metadata.</param>
        /// <returns>The <paramref name="settings" /> instance with <see cref="MinVerSettings.BuildMetadata" /> set to <paramref name="buildMetadata" />.</returns>
        public static MinVerSettings WithBuildMetadata(this MinVerSettings settings, string buildMetadata)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrWhiteSpace(buildMetadata))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(buildMetadata));
            }

            settings.BuildMetadata = buildMetadata;

            return settings;
        }

        /// <summary>
        /// Set the default pre-release phase.
        /// --default-pre-release-phase &lt;PHASE&gt;
        /// alpha (default), preview, etc.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="defaultPreReleasePhase">The build metadata.</param>
        /// <returns>The <paramref name="settings" /> instance with <see cref="MinVerSettings.DefaultPreReleasePhase" /> set to <paramref name="defaultPreReleasePhase" />.</returns>
        public static MinVerSettings WithDefaultPreReleasePhase(this MinVerSettings settings, string defaultPreReleasePhase)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrWhiteSpace(defaultPreReleasePhase))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(defaultPreReleasePhase));
            }

            settings.DefaultPreReleasePhase = defaultPreReleasePhase;

            return settings;
        }

        /// <summary>
        /// Set the minimum major and minor version.
        /// --minimum-major-minor &lt;MINIMUM_MAJOR_MINOR&gt;
        /// 1.0, 1.1, 2.0, etc.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="minimumMajorMinor">The build metadata.</param>
        /// <returns>The <paramref name="settings" /> instance with <see cref="MinVerSettings.MinimumMajorMinor" /> set to <paramref name="minimumMajorMinor" />.</returns>
        public static MinVerSettings WithMinimumMajorMinor(this MinVerSettings settings, string minimumMajorMinor)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrWhiteSpace(minimumMajorMinor))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(minimumMajorMinor));
            }

            settings.MinimumMajorMinor = minimumMajorMinor;

            return settings;
        }

        /// <summary>
        /// Set the working directory for MinVer to use.
        /// --repo &lt;REPO&gt;
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="repo">The working directory.</param>
        /// <returns>The <paramref name="settings" /> instance with <see cref="MinVerSettings.Repo" /> set to <paramref name="repo" />.</returns>
        public static MinVerSettings WithRepo(this MinVerSettings settings, DirectoryPath repo)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.Repo = repo ?? throw new ArgumentNullException(nameof(repo));

            return settings;
        }

        /// <summary>
        /// Set the tag prefix.
        /// --tag-prefix &lt;TAG_PREFIX&gt;
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="tagPrefix">The tag prefix.</param>
        /// <returns>The <paramref name="settings" /> instance with <see cref="MinVerSettings.Repo" /> set to <paramref name="tagPrefix" />.</returns>
        public static MinVerSettings WithTagPrefix(this MinVerSettings settings, string tagPrefix)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrWhiteSpace(tagPrefix))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(tagPrefix));
            }

            settings.TagPrefix = tagPrefix;

            return settings;
        }

        /// <summary>
        /// By default, MinVer is executed as a local tool first and, in case of error, fallback(*) to global tool
        /// Set <see cref="MinVerSettings.PreferGlobalTool" /> to <see langword="true" /> to execute MinVer as global tool first and,
        /// in case of an error, fallback(*) to local tool
        ///
        /// (*) Unless the fallback is disabled via <see cref="MinVerSettings.NoFallback" />
        ///
        /// Local tool = `dotnet minver`
        /// Global tool = `minver`
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The <paramref name="settings" /> instance with <see cref="MinVerSettings.Repo" />.</returns>
        public static MinVerSettings WithPreferGlobalTool(this MinVerSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.PreferGlobalTool = true;

            return settings;
        }

        /// <summary>
        /// By default, MinVer is executed as a local tool first(*) and, in case of error, fallback to global tool(*)
        /// Set <see cref="MinVerSettings.NoFallback" /> to <see langword="true" /> to disable the fallback in case of an error
        ///
        /// (*) Unless <see cref="MinVerSettings.PreferGlobalTool" /> is set to <see langword="true" />, in which case MinVer is
        /// executed as a global tool first and, in case of an error, fallback to local tool
        ///
        /// Local tool = `dotnet minver`
        /// Global tool = `minver`
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The <paramref name="settings" /> instance with <see cref="MinVerSettings.Repo" />.</returns>
        public static MinVerSettings WithNoFallback(this MinVerSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.NoFallback = true;

            return settings;
        }

        /// <summary>
        /// Set a custom path to the minver.exe file.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="toolPath">The custom path to the minver.exe file.</param>
        /// <returns>The <paramref name="settings" /> instance with <see cref="ToolSettings.ToolPath" /> set to <paramref name="toolPath" />.</returns>
        public static MinVerSettings WithToolPath(this MinVerSettings settings, FilePath toolPath)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.ToolPath = toolPath ?? throw new ArgumentNullException(nameof(toolPath));

            return settings;
        }

        /// <summary>
        /// Set the verbosity.
        /// --verbosity &lt;VERBOSITY&gt;
        /// error, warn, info (default), debug, or trace
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <returns>The <paramref name="settings" /> instance with <see cref="MinVerSettings.Verbosity" /> set to <paramref name="verbosity" />.</returns>
        public static MinVerSettings WithVerbosity(this MinVerSettings settings, MinVerVerbosity verbosity)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (!Enum.IsDefined(typeof(MinVerVerbosity), verbosity))
            {
                throw new InvalidEnumArgumentException(nameof(verbosity), (int)verbosity,
                    typeof(MinVerVerbosity));
            }

            settings.Verbosity = verbosity;

            return settings;
        }

        /// <summary>
        /// Sets the working directory which should be used to run the MinVer tool.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="path">Working directory which should be used to run the dotnet minver tool.</param>
        /// <returns>The <paramref name="settings" /> instance with <see cref="Core.Tooling.ToolSettings.WorkingDirectory" /> set to <paramref name="path" />.</returns>
        public static MinVerSettings FromPath(this MinVerSettings settings, DirectoryPath path)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.WorkingDirectory = path ?? throw new ArgumentNullException(nameof(path));

            return settings;
        }
    }
}
