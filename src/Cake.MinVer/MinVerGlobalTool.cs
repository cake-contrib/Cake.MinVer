using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.MinVer
{
    internal class MinVerGlobalTool : MinVerToolBase
    {
        public MinVerGlobalTool(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            ICakeLog log) : base(fileSystem, environment, processRunner, tools, log)
        {
        }

        /// <inheritdoc />
        protected override ProcessArgumentBuilder GetArguments(MinVerSettings settings)
        {
            var command = new ProcessArgumentBuilder();
            var args = CreateArgumentBuilder(settings);

            if (!args.IsNullOrEmpty())
            {
                args.CopyTo(command);
            }

            CakeLog.Verbose("{0} arguments: {1}", GetToolName(), args.RenderSafe());

            return command;
        }

        /// <inheritdoc />
        protected override string GetToolName()
        {
            return "MinVer Global Tool (minver)";
        }

        /// <inheritdoc />
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new [] { "minver", "minver.exe" };
        }
    }
}
