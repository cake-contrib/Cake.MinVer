using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.MinVer
{
    internal class MinVerLocalTool : MinVerToolBase
    {
        public MinVerLocalTool(
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

            command.Append("minver");

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
            return "MinVer Local Tool (dotnet minver)";
        }
    }
}
