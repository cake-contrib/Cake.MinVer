using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Fixtures;

namespace Cake.MinVer.Tests.Support
{
    internal class MinVerToolFixture<T> : ToolFixture<MinVerSettings, ToolFixtureResult>
        where T : MinVerToolBase
    {
        public ICakeLog Log = new FakeLog();

        public MinVerToolFixture(string toolFilename = null)
            : base(toolFilename ?? "dotnet.exe")
        {
            ProcessRunner.Process.SetStandardOutput(MinVerToolOutputs.OutputWhenNotAGitRepo);
        }

        public IEnumerable<string> StandardOutput
        {
            set => ProcessRunner.Process.SetStandardOutput(value);
        }

        public T Tool { get; protected set; }
        public MinVerVersion Result { get; private set; }

        protected override void RunTool()
        {
            var exitCode = Tool.TryRun(Settings, out var result);
            Result = exitCode == 0 ? result : null;
        }

        protected override ToolFixtureResult CreateResult(FilePath path, ProcessSettings process)
        {
            return new ToolFixtureResult(path, process);
        }
    }
}
