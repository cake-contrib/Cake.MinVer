using System;
using Cake.Core;
using Cake.Testing;
using FluentAssertions;
using Xunit;

namespace Cake.MinVer.Tests
{
    public sealed class MinVerToolTests
    {
        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            var fixture = new MinVerToolFixture
            {
                Settings = null,
            };

            fixture.GivenDefaultToolDoNotExist();

            fixture.Invoking(f => f.Run())
                .Should().ThrowExactly<ArgumentNullException>()
                .And.ParamName.Should().Be("settings");
        }

        [Fact]
        public void Should_Add_Mandatory_Arguments()
        {
            var fixture = new MinVerToolFixture();
            var result = fixture.Run();

            result.Args.Should().Be("minver");
        }

        [Theory]
        [InlineData(MinVerAutoIncrement.Default, "minver")]
        [InlineData(MinVerAutoIncrement.Major, "minver --auto-increment major")]
        [InlineData(MinVerAutoIncrement.Minor, "minver --auto-increment minor")]
        [InlineData(MinVerAutoIncrement.Patch, "minver --auto-increment patch")]
        public void Should_Add_Auto_Increment_To_Arguments(MinVerAutoIncrement autoIncrement, string expectedArgs)
        {
            var fixture = new MinVerToolFixture
            {
                Settings =
                {
                    AutoIncrement = autoIncrement,
                },
            };

            var result = fixture.Run();

            result.Args.Should().Be(expectedArgs);
        }

        [Fact]
        public void Should_Add_Build_Metadata_Arguments()
        {
            var fixture = new MinVerToolFixture
            {
                Settings =
                {
                    BuildMetadata = "1234abc",
                },
            };

            var result = fixture.Run();

            result.Args.Should().Be("minver --build-metadata \"1234abc\"");
        }

        [Fact]
        public void Should_Add_Default_Pre_Release_Phase_Arguments()
        {
            var fixture = new MinVerToolFixture
            {
                Settings =
                {
                    DefaultPreReleasePhase = "preview",
                },
            };

            var result = fixture.Run();

            result.Args.Should().Be("minver --default-pre-release-phase \"preview\"");
        }

        [Fact]
        public void Should_Add_Minimum_Major_Minor_Arguments()
        {
            var fixture = new MinVerToolFixture
            {
                Settings =
                {
                    MinimumMajorMinor = "2.0",
                },
            };

            var result = fixture.Run();

            result.Args.Should().Be("minver --minimum-major-minor \"2.0\"");
        }

        [Fact]
        public void Should_Add_Repo_Arguments()
        {
            var fixture = new MinVerToolFixture
            {
                Settings =
                {
                    Repo = "./src",
                },
            };

            var result = fixture.Run();

            result.Args.Should().Be("minver --repo \"src\"");
        }

        [Fact]
        public void Should_Add_Tag_Prefix_Arguments()
        {
            var fixture = new MinVerToolFixture
            {
                Settings =
                {
                    TagPrefix = "v",
                },
            };

            var result = fixture.Run();

            result.Args.Should().Be("minver --tag-prefix \"v\"");
        }

        [Theory]
        [InlineData(MinVerVerbosity.Default, "minver")]
        [InlineData(MinVerVerbosity.Error, "minver --verbosity error")]
        [InlineData(MinVerVerbosity.Warn, "minver --verbosity warn")]
        [InlineData(MinVerVerbosity.Info, "minver --verbosity info")]
        [InlineData(MinVerVerbosity.Debug, "minver --verbosity debug")]
        [InlineData(MinVerVerbosity.Trace, "minver --verbosity trace")]
        public void Should_Add_Verbosity_To_Arguments(MinVerVerbosity verbosity, string expectedArgs)
        {
            var fixture = new MinVerToolFixture
            {
                Settings =
                {
                    Verbosity = verbosity,
                },
            };

            var result = fixture.Run();

            result.Args.Should().Be(expectedArgs);
        }

        [Fact]
        public void Should_Throw_Cake_Exception_If_Cant_Parse_MinVer_Version()
        {
            var fixture = new MinVerToolFixture
            {
                StandardOutput = new [] { "abcd" },
            };

            fixture.Invoking(f => f.Run())
                .Should().ThrowExactly<CakeException>()
                .WithMessage("Version 'abcd' is not valid.");
        }
    }
}
