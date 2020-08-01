using System;
using System.Linq;
using Cake.Common.Tools.DotNetCore;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.MinVer
{
    /// <summary>
    /// MinVer dotnet tool.
    /// </summary>
    public class MinVerTool : DotNetCoreTool<MinVerSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinVerTool" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="log">Cake log instance.</param>
        public MinVerTool(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            ICakeLog log) : base(fileSystem, environment, processRunner, tools)
        {
            CakeLog = log ?? throw new ArgumentNullException(nameof(log));
        }

        /// <summary>
        /// Cake log instance.
        /// </summary>
        public ICakeLog CakeLog { get; }

        /// <summary>
        /// Run the MinVer dotnet tool using the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public MinVerVersion Run(MinVerSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var processSettings = new ProcessSettings
            {
                RedirectStandardOutput = true,
            };

            MinVerVersion minVerVersion = null;

            Run(settings, GetArguments(settings), processSettings, p => minVerVersion = ParseVersion(p));

            return minVerVersion;
        }

        /// <inheritdoc />
        protected override string GetToolName()
        {
            return "MinVer";
        }

        private MinVerVersion ParseVersion(IProcess process)
        {
            if (process.GetExitCode() != 0) return null;

            var version = process.GetStandardOutput().LastOrDefault();
            if (string.IsNullOrWhiteSpace(version))
            {
                throw new CakeException($"Version '{version}' is not valid.");
            }

            try
            {
                return new MinVerVersion(version);
            }
            catch (Exception ex)
            {
                throw new CakeException($"Version '{version}' is not valid.", ex);
            }
        }

        private ProcessArgumentBuilder GetArguments(MinVerSettings settings)
        {
            var command = new ProcessArgumentBuilder();
            var args = CreateArgumentBuilder(settings);

            command.Append("minver");

            if (!args.IsNullOrEmpty())
            {
                args.CopyTo(command);
            }

            CakeLog.Verbose("dotnet minver arguments: {0}", args.RenderSafe());

            return command;
        }

        /// <summary>
        /// Creates a <see cref="ProcessArgumentBuilder" /> and adds common commandline arguments.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>Instance of <see cref="ProcessArgumentBuilder" />.</returns>
        protected new ProcessArgumentBuilder CreateArgumentBuilder(MinVerSettings settings)
        {
            var args = base.CreateArgumentBuilder(settings);

            AppendAutoIncrement(args, settings);

            if (!string.IsNullOrWhiteSpace(settings.BuildMetadata))
            {
                args.Append("--build-metadata");
                args.AppendQuoted(settings.BuildMetadata);
            }

            if (!string.IsNullOrWhiteSpace(settings.DefaultPreReleasePhase))
            {
                args.Append("--default-pre-release-phase");
                args.AppendQuoted(settings.DefaultPreReleasePhase);
            }

            if (!string.IsNullOrWhiteSpace(settings.MinimumMajorMinor))
            {
                args.Append("--minimum-major-minor");
                args.AppendQuoted(settings.MinimumMajorMinor);
            }

            if (!string.IsNullOrWhiteSpace(settings.Repo?.FullPath))
            {
                args.Append("--repo");
                args.AppendQuoted(settings.Repo.FullPath);
            }

            if (!string.IsNullOrWhiteSpace(settings.TagPrefix))
            {
                args.Append("--tag-prefix");
                args.AppendQuoted(settings.TagPrefix);
            }

            AppendVerbosity(args, settings);

            return args;
        }

        private static void AppendAutoIncrement(ProcessArgumentBuilder args, MinVerSettings settings)
        {
            switch (settings.AutoIncrement)
            {
                case MinVerAutoIncrement.Default:
                    break;

                case MinVerAutoIncrement.Major:
                    args.Append("--auto-increment major");
                    break;

                case MinVerAutoIncrement.Minor:
                    args.Append("--auto-increment minor");
                    break;

                case MinVerAutoIncrement.Patch:
                    args.Append("--auto-increment patch");
                    break;

                default:
                    throw new CakeException($"{nameof(settings.AutoIncrement)}={(int)settings.AutoIncrement} is invalid");
            }
        }

        private static void AppendVerbosity(ProcessArgumentBuilder args, MinVerSettings settings)
        {
            var verbosity = settings.Verbosity;
            var toolVerbosity = settings.ToolVerbosity;

            if (verbosity == MinVerVerbosity.Default && toolVerbosity.HasValue)
            {
                verbosity = ToolToMinVerVerbosityConverter(toolVerbosity.Value);
            }

            switch (verbosity)
            {
                case MinVerVerbosity.Default:
                    break;

                case MinVerVerbosity.Error:
                    args.Append("--verbosity error");
                    break;

                case MinVerVerbosity.Warn:
                    args.Append("--verbosity warn");
                    break;

                case MinVerVerbosity.Info:
                    args.Append("--verbosity info");
                    break;

                case MinVerVerbosity.Debug:
                    args.Append("--verbosity debug");
                    break;

                case MinVerVerbosity.Trace:
                    args.Append("--verbosity trace");
                    break;

                default:
                    throw new CakeException($"{nameof(settings.Verbosity)}={(int)verbosity} is invalid");
            }
        }

        private static MinVerVerbosity ToolToMinVerVerbosityConverter(DotNetCoreVerbosity toolVerbosity)
        {
            return toolVerbosity switch
            {
                DotNetCoreVerbosity.Quiet => MinVerVerbosity.Error,
                DotNetCoreVerbosity.Minimal => MinVerVerbosity.Warn,
                DotNetCoreVerbosity.Normal => MinVerVerbosity.Info,
                DotNetCoreVerbosity.Detailed => MinVerVerbosity.Debug,
                DotNetCoreVerbosity.Diagnostic => MinVerVerbosity.Trace,
                _ => MinVerVerbosity.Default
            };
        }
    }
}
