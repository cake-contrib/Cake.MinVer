using System;
using System.Globalization;
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
        private readonly MinVerLocalTool _localTool;
        private readonly MinVerGlobalTool _globalTool;

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
            ICakeLog log) : this(fileSystem, environment, processRunner, tools, log, localTool: null, globalTool: null)
        {
        }

        internal MinVerTool(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            ICakeLog log,
            MinVerLocalTool localTool,
            MinVerGlobalTool globalTool) : base(fileSystem, environment, processRunner, tools)
        {
            CakeLog = log ?? throw new ArgumentNullException(nameof(log));

            _localTool = localTool ?? new MinVerLocalTool(fileSystem, environment, processRunner, tools, log);
            _globalTool = globalTool ?? new MinVerGlobalTool(fileSystem, environment, processRunner, tools, log);
        }

        /// <summary>
        /// Cake log instance.
        /// </summary>
        public ICakeLog CakeLog { get; }

        /// <summary>
        /// Run the MinVer dotnet tool using the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The MinVer calculated version information</returns>
        public MinVerVersion Run(MinVerSettings settings)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (!(settings.ToolPath is null))
            {
                // If ToolPath is specified, it means it's a global tool in a custom location (also known as a tool-path tool)
                // https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools
                settings.PreferGlobalTool = true;

                // If ToolPath is specified, we try to run that specific tool only... No fallback
                settings.NoFallback = true;
            }

            MinVerToolBase preferredTool;
            MinVerToolBase fallbackTool;

            if (settings.PreferGlobalTool)
            {
                preferredTool = _globalTool;
                fallbackTool = _localTool;
            }
            else
            {
                preferredTool = _localTool;
                fallbackTool = _globalTool;
            }

            var preferredToolExitCode = preferredTool.TryRun(settings, out var minVerVersion);
            if (preferredToolExitCode == 0)
            {
                return minVerVersion;
            }

            if (settings.NoFallback)
            {
                ProcessExitCode(preferredToolExitCode);
                return null;
            }

            var fallbackToolExitCode = fallbackTool.TryRun(settings, out minVerVersion);
            if (fallbackToolExitCode == 0)
            {
                CakeLog.Verbose(string.Format(CultureInfo.InvariantCulture,
                    "{0}: Process returned an error (exit code {1}), but {2} executed successfully.",
                    preferredTool.ToolName, preferredToolExitCode, fallbackTool.ToolName));

                ProcessExitCode(fallbackToolExitCode);
                return minVerVersion;
            }

            CakeLog.Verbose(string.Format(CultureInfo.InvariantCulture,
                "{0}: Process returned an error (exit code {1}).", preferredTool.ToolName, preferredToolExitCode));

            CakeLog.Verbose(string.Format(CultureInfo.InvariantCulture,
                "{0}: Process returned an error (exit code {1}).", fallbackTool.ToolName, fallbackToolExitCode));

            ProcessExitCode(preferredToolExitCode);
            return null;
        }

        /// <inheritdoc />
        protected override string GetToolName()
        {
            return "MinVer";
        }
    }
}
