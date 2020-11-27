using System;
using System.Linq;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Fixtures;

namespace Cake.MinVer.Tests.Support
{
    internal sealed class MinVerToolFixture : ToolFixture<MinVerSettings, MinVerToolFixtureResult>
    {
        public MinVerToolFixture(ICakeLog log)
            : base("dummy.exe")
        {
            Log = log ?? throw new ArgumentNullException(nameof(log));

            var context = new MinVerToolContext();

            LocalTool = new MinVerLocalToolFixture(this, context);
            GlobalTool = new MinVerGlobalToolFixture(this, context);

            FileSystem.CreateFile(LocalTool.DefaultToolPath);
            FileSystem.CreateFile(GlobalTool.DefaultToolPath);
        }

        public ICakeLog Log { get; }

        public MinVerLocalToolFixture LocalTool {  get; }

        public MinVerGlobalToolFixture GlobalTool { get; }

        public void GivenLocalToolFailsToRun()
        {
            LocalTool.ProcessRunner.Process.SetExitCode(1);
        }

        public void GivenGlobalToolFailsToRun()
        {
            GlobalTool.ProcessRunner.Process.SetExitCode(1);
        }

        public void GivenLocalToolIsNotInstalled()
        {
            FileSystem.EnsureFileDoesNotExist(LocalTool.DefaultToolPath);
        }

        public void GivenGlobalToolIsNotInstalled()
        {
            FileSystem.EnsureFileDoesNotExist(GlobalTool.DefaultToolPath);
        }

        public new MinVerToolFixtureResult Run()
        {
            var tool = new MinVerTool(FileSystem, Environment, ProcessRunner, Tools, Log, LocalTool, GlobalTool);
            var version = tool.Run(Settings);

            var results = new []
            {
                new
                {
                    ExecutionOrder = LocalTool.ExecutionOrder,
                    ProcessResult = LocalTool.ProcessRunner.Results.LastOrDefault(),
                },
                new
                {
                    ExecutionOrder = GlobalTool.ExecutionOrder,
                    ProcessResult = GlobalTool.ProcessRunner.Results.LastOrDefault(),
                }
            };

            var finalResult = results.OrderBy(r => r.ExecutionOrder).LastOrDefault()?.ProcessResult;
            if (!(finalResult is null))
            {
                finalResult.Version = version;
            }

            return finalResult;
        }

        protected override void RunTool()
        {
            // Implemented via (new) Run above
            throw new NotImplementedException();
        }

        protected override MinVerToolFixtureResult CreateResult(FilePath path, ProcessSettings process)
        {
            // Implemented in the individual tools as they have separate ProcessRunner instances
            throw new NotImplementedException();
        }
    }
}
