#region Copyright 2020-2023 C. Augusto Proiete & Contributors
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
using Xunit;
using Cake.Common.Tools.DotNet;
using Cake.Core.IO;
using VerifyXunit;
using System.Threading.Tasks;

namespace Cake.MinVer.Tests;

[UsesVerify]
public sealed class MinVerSettingsTests
{
    [Fact]
    public Task Should_Shallow_Clone_All_Properties()
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
            ToolVerbosity = DotNetVerbosity.Detailed,

            DiagnosticOutput = true,
            ToolPath = FilePath.FromString("/tools/custom/minver.exe"),
            ToolTimeout = TimeSpan.FromMinutes(5),
            WorkingDirectory = DirectoryPath.FromString("/working/folder"),
            NoWorkingDirectory = true,
            ArgumentCustomization = s => s,
            EnvironmentVariables = new Dictionary<string, string> { { "MINVERTESTVAR", "SOMEVALUE" } },
            HandleExitCode = i => false,
        };
        return Verifier.Verify(expected.Clone());
    }
}
