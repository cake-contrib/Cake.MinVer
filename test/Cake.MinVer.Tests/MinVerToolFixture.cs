using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Fixtures;

namespace Cake.MinVer.Tests
{
    internal sealed class MinVerToolFixture : ToolFixture<MinVerSettings, ToolFixtureResult>
    {
        private readonly ICakeLog _log = new FakeLog();

        public MinVerToolFixture()
            : base("dotnet.exe")
        {
            ProcessRunner.Process.SetStandardOutput(new[] { "0.0.0-alpha.0" });
        }

        public IEnumerable<string> StandardOutput
        {
            set => ProcessRunner.Process.SetStandardOutput(value);
        }

        protected override void RunTool()
        {
            var tool = new MinVerTool(FileSystem, Environment, ProcessRunner, Tools, _log);
            tool.Run(Settings);
        }

        protected override ToolFixtureResult CreateResult(FilePath path, ProcessSettings process)
        {
            return new ToolFixtureResult(path, process);
        }
    }
}
