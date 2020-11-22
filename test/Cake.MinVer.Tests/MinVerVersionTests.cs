using System;
using FluentAssertions;
using Xunit;

namespace Cake.MinVer.Tests
{
    public sealed class MinVerVersionTests
    {
        [Fact]
        public void Should_Throw_If_Version_is_Null()
        {
            Action fixture = () =>
            {
                var _ = new MinVerVersion(version: null);
            };

            fixture.Should().ThrowExactly<ArgumentException>()
                .And.ParamName.Should().Be("version");
        }

        [Fact]
        public void Should_Parse_Major_Version()
        {
            new MinVerVersion("1.2.3").Major.Should().Be(1);
            new MinVerVersion("1.2.3-alpha").Major.Should().Be(1);
            new MinVerVersion("1.2.3-alpha.4").Major.Should().Be(1);
            new MinVerVersion("1.2.3-alpha.4+abcdefg").Major.Should().Be(1);
        }

        [Fact]
        public void Should_Parse_Minor_Version()
        {
            new MinVerVersion("1.2.3").Minor.Should().Be(2);
            new MinVerVersion("1.2.3-alpha").Minor.Should().Be(2);
            new MinVerVersion("1.2.3-alpha.4").Minor.Should().Be(2);
            new MinVerVersion("1.2.3-alpha.4+abcdefg").Minor.Should().Be(2);
        }

        [Fact]
        public void Should_Parse_Patch_Version()
        {
            new MinVerVersion("1.2.3").Patch.Should().Be(3);
            new MinVerVersion("1.2.3-alpha").Patch.Should().Be(3);
            new MinVerVersion("1.2.3-alpha.4").Patch.Should().Be(3);
            new MinVerVersion("1.2.3-alpha.4+abcdefg").Patch.Should().Be(3);
        }

        [Fact]
        public void Should_Parse_PreRelease_Extension()
        {
            new MinVerVersion("1.2.3").PreRelease.Should().BeNull();
            new MinVerVersion("1.2.3-alpha").PreRelease.Should().Be("alpha");
            new MinVerVersion("1.2.3-alpha.4").PreRelease.Should().Be("alpha.4");
            new MinVerVersion("1.2.3-alpha.4+abcdefg").PreRelease.Should().Be("alpha.4");
        }

        [Fact]
        public void Should_Parse_BuildMetadata_Extension()
        {
            new MinVerVersion("1.2.3").BuildMetadata.Should().BeNull();
            new MinVerVersion("1.2.3-alpha").BuildMetadata.Should().BeNull();
            new MinVerVersion("1.2.3-alpha.4").BuildMetadata.Should().BeNull();
            new MinVerVersion("1.2.3-alpha.4+abcdefg").BuildMetadata.Should().Be("abcdefg");
        }

        [Fact]
        public void Should_Set_AssemblyVersion()
        {
            new MinVerVersion("1.2.3").AssemblyVersion.Should().Be("1.0.0.0");
            new MinVerVersion("1.2.3-alpha").AssemblyVersion.Should().Be("1.0.0.0");
            new MinVerVersion("1.2.3-alpha.4").AssemblyVersion.Should().Be("1.0.0.0");
            new MinVerVersion("1.2.3-alpha.4+abcdefg").AssemblyVersion.Should().Be("1.0.0.0");
        }

        [Fact]
        public void Should_Set_FileVersion()
        {
            new MinVerVersion("1.2.3").FileVersion.Should().Be("1.2.3.0");
            new MinVerVersion("1.2.3-alpha").FileVersion.Should().Be("1.2.3.0");
            new MinVerVersion("1.2.3-alpha.4").FileVersion.Should().Be("1.2.3.0");
            new MinVerVersion("1.2.3-alpha.4+abcdefg").FileVersion.Should().Be("1.2.3.0");
        }

        [Fact]
        public void Should_Set_InformationalVersion()
        {
            new MinVerVersion("1.2.3").InformationalVersion.Should().Be("1.2.3");
            new MinVerVersion("1.2.3-alpha").InformationalVersion.Should().Be("1.2.3-alpha");
            new MinVerVersion("1.2.3-alpha.4").InformationalVersion.Should().Be("1.2.3-alpha.4");
            new MinVerVersion("1.2.3-alpha.4+abcdefg").InformationalVersion.Should().Be("1.2.3-alpha.4+abcdefg");
        }

        [Fact]
        public void Should_Set_PackageVersion()
        {
            new MinVerVersion("1.2.3").PackageVersion.Should().Be("1.2.3");
            new MinVerVersion("1.2.3-alpha").PackageVersion.Should().Be("1.2.3-alpha");
            new MinVerVersion("1.2.3-alpha.4").PackageVersion.Should().Be("1.2.3-alpha.4");
            new MinVerVersion("1.2.3-alpha.4+abcdefg").PackageVersion.Should().Be("1.2.3-alpha.4+abcdefg");
        }

        [Fact]
        public void Should_Set_Version()
        {
            new MinVerVersion("1.2.3-alpha.4+abcdefg").Version.Should().Be("1.2.3-alpha.4+abcdefg");
            new MinVerVersion("1.2.3-Alpha.4+abcdefg").Version.Should().Be("1.2.3-Alpha.4+abcdefg");
            new MinVerVersion("1.2.3-alpha.4+Abcdefg").Version.Should().Be("1.2.3-alpha.4+Abcdefg");
        }

        [Fact]
        public void Should_Convert_to_string_implicitly()
        {
            string versionString = new MinVerVersion("1.2.3-alpha.4+abcdefg");
            versionString.Should().Be("1.2.3-alpha.4+abcdefg");
        }

        [Fact]
        public void Should_Set_IsPreRelease()
        {
            new MinVerVersion("1.2.3").IsPreRelease.Should().Be(false);
            new MinVerVersion("1.2.3-alpha").IsPreRelease.Should().Be(true);
            new MinVerVersion("1.2.3-alpha.4").IsPreRelease.Should().Be(true);
            new MinVerVersion("1.2.3-alpha.4+abcdefg").IsPreRelease.Should().Be(true);
        }
    }
}
