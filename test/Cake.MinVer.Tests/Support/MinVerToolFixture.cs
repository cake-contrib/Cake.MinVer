using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Fixtures;

namespace Cake.MinVer.Tests.Support
{
    internal sealed class MinVerToolFixture : ToolFixture<MinVerSettings, ToolFixtureResult>
    {
        private readonly ICakeLog _log = new FakeLog();

        public MinVerToolFixture(string toolFilename = null)
            : base(toolFilename ?? "dotnet.exe")
        {
            ProcessRunner.Process.SetStandardOutput(MinVerToolOutputs.OutputWhenNotAGitRepo);
        }

        public IEnumerable<string> StandardOutput
        {
            set => ProcessRunner.Process.SetStandardOutput(value);
        }

        public MinVerLocalToolFixture LocalTool { private get; set; }
        public MinVerGlobalToolFixture GlobalTool { private get; set; }

        public MinVerVersion Result { get; private set; }

        protected override void RunTool()
        {
            var tool = new MinVerTool(FileSystem, Environment, ProcessRunner, Tools, _log, LocalTool.Tool, GlobalTool.Tool);
            Result = tool.Run(Settings);
        }

        protected override ToolFixtureResult CreateResult(FilePath path, ProcessSettings process)
        {
            return new ToolFixtureResult(path, process);
        }
    }
}
