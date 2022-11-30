#region Copyright 2020-2022 C. Augusto Proiete & Contributors
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

using FluentAssertions;
using Xunit;

namespace Cake.MinVer.Tests
{
    public sealed class MinVerSettingsExtensionsTests
    {
        [Fact]
        public void Should_Set_AutoIncrement_via_WithAutoIncrement()
        {
            var settings = new MinVerSettings()
                .WithAutoIncrement(MinVerAutoIncrement.Major);

            settings.AutoIncrement.Should().Be(MinVerAutoIncrement.Major);
        }

        [Fact]
        public void Should_Set_BuildMetadata_via_WithBuildMetadata()
        {
            var settings = new MinVerSettings()
                .WithBuildMetadata("abcdefg");

            settings.BuildMetadata.Should().Be("abcdefg");
        }

        [Fact]
        public void Should_Set_DefaultPreReleasePhase_via_WithDefaultPreReleasePhase()
        {
            var settings = new MinVerSettings()
                .WithDefaultPreReleasePhase("preview");

            settings.DefaultPreReleasePhase.Should().Be("preview");
        }

        [Fact]
        public void Should_Set_MinimumMajorMinor_via_WithMinimumMajorMinor()
        {
            var settings = new MinVerSettings()
                .WithMinimumMajorMinor("2.5");

            settings.MinimumMajorMinor.Should().Be("2.5");
        }

        [Fact]
        public void Should_Set_Repo_via_WithRepo()
        {
            var settings = new MinVerSettings()
                .WithRepo("./src");

            settings.Repo.FullPath.Should().Be("src");
        }

        [Fact]
        public void Should_Set_TagPrefix_via_WithTagPrefix()
        {
            var settings = new MinVerSettings()
                .WithTagPrefix("v");

            settings.TagPrefix.Should().Be("v");
        }

        [Fact]
        public void Should_Set_PreferGlobalTool_via_WithPreferGlobalTool()
        {
            var settings = new MinVerSettings()
                .WithPreferGlobalTool();

            settings.PreferGlobalTool.Should().BeTrue();
        }

        [Fact]
        public void Should_Set_NoFallback_via_WithNoFallback()
        {
            var settings = new MinVerSettings()
                .WithNoFallback();

            settings.NoFallback.Should().BeTrue();
        }

        [Fact]
        public void Should_Set_ToolPath_via_WithToolPath()
        {
            var settings = new MinVerSettings()
                .WithToolPath(@"c:\myCustomTools\minver.exe");

            settings.ToolPath.FullPath.Should().Be(@"c:/myCustomTools/minver.exe");
        }

        [Fact]
        public void Should_Set_Verbosity_via_WithVerbosity()
        {
            var settings = new MinVerSettings()
                .WithVerbosity(MinVerVerbosity.Trace);

            settings.Verbosity.Should().Be(MinVerVerbosity.Trace);
        }

        [Fact]
        public void Should_Set_WorkingDirectory_via_FromPath()
        {
            var settings = new MinVerSettings()
                .FromPath("./src");

            settings.WorkingDirectory.FullPath.Should().Be("src");
        }
    }
}
