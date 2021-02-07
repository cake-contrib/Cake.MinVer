#region Copyright 2020-2021 C. Augusto Proiete & Contributors
//
// Licensed under the MIT (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://opensource.org/licenses/MIT
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

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
