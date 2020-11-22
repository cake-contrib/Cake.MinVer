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
