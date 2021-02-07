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
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Cake.Common.Tools.DotNetCore;
using Cake.Core.IO;

namespace Cake.MinVer.Tests
{
    public sealed class MinVerSettingsTests
    {
        [Fact]
        public void Should_Shallow_Clone_All_Properties()
        {
            var expected = new MinVerSettings
            {
                AutoIncrement = MinVerAutoIncrement.Minor,
                BuildMetadata = "123",
                DefaultPreReleasePhase = "preview",
                MinimumMajorMinor = "1.2",
                Repo = DirectoryPath.FromString("/repo/custom"),
                TagPrefix = "v",
                PreferGlobalTool = true,
                NoFallback = true,
                Verbosity = MinVerVerbosity.Trace,
                ToolVerbosity = DotNetCoreVerbosity.Detailed,

                DiagnosticOutput = true,
                ToolPath = FilePath.FromString("/tools/custom/minver.exe"),
                ToolTimeout = TimeSpan.FromMinutes(5),
                WorkingDirectory = DirectoryPath.FromString("/working/folder"),
                NoWorkingDirectory = true,
                ArgumentCustomization = s => s,
                EnvironmentVariables = new Dictionary<string, string> { { "MINVERTESTVAR", "SOMEVALUE" } },
                HandleExitCode = i => false,
            };

            var actual = expected.Clone();

            actual.AutoIncrement.Should().Be(expected.AutoIncrement);
            actual.BuildMetadata.Should().Be(expected.BuildMetadata);
            actual.DefaultPreReleasePhase.Should().Be(expected.DefaultPreReleasePhase);
            actual.MinimumMajorMinor.Should().Be(expected.MinimumMajorMinor);
            actual.Repo.Should().Be(expected.Repo);
            actual.TagPrefix.Should().Be(expected.TagPrefix);
            actual.PreferGlobalTool.Should().Be(expected.PreferGlobalTool);
            actual.NoFallback.Should().Be(expected.NoFallback);
            actual.Verbosity.Should().Be(expected.Verbosity);
            actual.ToolVerbosity.Should().Be(expected.ToolVerbosity);

            actual.DiagnosticOutput.Should().Be(expected.DiagnosticOutput);
            actual.ToolPath.Should().Be(expected.ToolPath);
            actual.ToolTimeout.Should().Be(expected.ToolTimeout);
            actual.WorkingDirectory.Should().Be(expected.WorkingDirectory);
            actual.NoWorkingDirectory.Should().Be(expected.NoWorkingDirectory);
            actual.ArgumentCustomization.Should().Be(expected.ArgumentCustomization);
            actual.EnvironmentVariables.Should().BeEquivalentTo(expected.EnvironmentVariables);
            actual.HandleExitCode.Should().BeEquivalentTo(expected.HandleExitCode);

            var properties = typeof(MinVerSettings)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Select(p => p.Name)
                .ToList();

            // Sanity check & alarm to detect changes in properties that need to be considered in the cloning
            properties.Should().HaveCount(18);
        }
    }
}
