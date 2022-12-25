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

using System;
using FluentAssertions;
using Xunit;

namespace Cake.MinVer.Tests;

public sealed class MinVerVersionTests
{
    [Fact]
    public void Should_Throw_If_Version_is_Null()
    {
        Action fixture = () =>
        {
            var _ = new MinVerVersion(versionString: null);
        };

        fixture.Should().ThrowExactly<ArgumentException>()
            .And.ParamName.Should().Be("versionString");
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

    [Theory]
    [InlineData("1.2.3")]
    [InlineData("1.2.3-alpha")]
    [InlineData("1.2.3-alpha.4")]
    [InlineData("1.2.3-alpha.4+abcdefg")]
    public void Should_Parse_Valid_Version_Strings(string versionString)
    {
        var version1 = new MinVerVersion(versionString);
        var version2 = MinVerVersion.Parse(versionString);
        var result = MinVerVersion.TryParse(versionString, out var version3);

        result.Should().BeTrue();

        version1.Version.Should().Be(versionString);
        version2.Version.Should().Be(versionString);
        version3.Version.Should().Be(versionString);

        version1.Should().BeEquivalentTo(version2);
        version2.Should().BeEquivalentTo(version3);
    }

    [Theory]
    [InlineData("x.2.3")]
    [InlineData("1x2.3")]
    [InlineData("1.x.3")]
    [InlineData("1.2x3")]
    [InlineData("1.2.x")]
    [InlineData("1.2.3xalpha")]
    public void Should_Not_Parse_Invalid_Version_Strings(string versionString)
    {
        Action version1Fixture = () => { var _ = new MinVerVersion(versionString); };
        Action version2Fixture = () => { var _ = MinVerVersion.Parse(versionString); };
        var result = MinVerVersion.TryParse(versionString, out _);

        version1Fixture.Should().ThrowExactly<FormatException>();
        version2Fixture.Should().ThrowExactly<FormatException>();
        result.Should().BeFalse();
    }
}
