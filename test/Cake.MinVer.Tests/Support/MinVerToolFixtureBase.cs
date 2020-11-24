using System;
using System.Collections.Generic;
using Xunit.Sdk;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.MinVer.Tests.Support
{
    internal abstract class MinVerToolFixtureBase<TTool> : ToolFixture<MinVerSettings, MinVerToolFixtureResult>, IMinVerTool
        where TTool : class, IMinVerTool
    {
        // ReSharper disable InconsistentNaming
        protected readonly MinVerToolContext _context;
        protected TTool _tool;
        // ReSharper restore InconsistentNaming

        protected MinVerToolFixtureBase(MinVerToolContext context)
            : base("dummy.exe")
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<string> StandardOutput
        {
            set => ProcessRunner.Process.SetStandardOutput(value);
        }

        public string ToolName => _tool.ToolName;

        public new abstract FilePath DefaultToolPath { get; }

        public int ExecutionOrder { get; private set; }

        public int ExecutionCount { get; private set; }

        public int TryRun(MinVerSettings settings, out MinVerVersion version)
        {
            ExecutionOrder = _context.GetExecutionOrder();
            ExecutionCount++;

            return _tool.TryRun(settings, out version);
        }

        public void ShouldHaveRunFirst()
        {
            if (ExecutionOrder != 1)
            {
                throw new XunitException($"Expected {GetType().Name} {nameof(ExecutionOrder)} to be 1, but found {ExecutionOrder}.");
            }
        }

        public void ShouldHaveRunSecond()
        {
            if (ExecutionOrder != 2)
            {
                throw new XunitException($"Expected {GetType().Name} {nameof(ExecutionOrder)} to be 2, but found {ExecutionOrder}.");
            }
        }

        public void ShouldHaveRunOnce()
        {
            if (ExecutionCount != 1)
            {
                throw new XunitException($"Expected {GetType().Name} {nameof(ExecutionCount)} to be 1, but found {ExecutionCount}.");
            }
        }

        public void ShouldNotHaveRun()
        {
            if (ExecutionCount != 0)
            {
                throw new XunitException($"Expected {GetType().Name} {nameof(ExecutionCount)} to be 0, but found {ExecutionCount}.");
            }
        }

        protected override MinVerToolFixtureResult CreateResult(FilePath path, ProcessSettings process)
        {
            return new MinVerToolFixtureResult(path, process);
        }

        protected override void RunTool()
        {
            // RunTool is called in the main Fixture
            throw new NotImplementedException();
        }
    }
}
