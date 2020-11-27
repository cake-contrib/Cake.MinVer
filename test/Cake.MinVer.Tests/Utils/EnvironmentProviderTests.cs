using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using Cake.Core;
using Cake.MinVer.Utils;
using Cake.Testing;

namespace Cake.MinVer.Tests.Utils
{
    public sealed class EnvironmentProviderTests
    {
        [Theory]
        [InlineData("MINVERTESTVAR", null, null, null)]
        [InlineData("MINVERTESTVAR", "", null, null)]
        [InlineData("MINVERTESTVAR", " ", null, null)]
        [InlineData("MINVERTESTVAR", null, "1", "1")]
        [InlineData("MINVERTESTVAR", "1",  null, "1")]
        public void Can_Get_Environment_Variable_As_String(string name, string value, string defaultValue, string expectedValue)
        {
            var cakeEnvironment = new FakeEnvironment(PlatformFamily.Windows, is64Bit: true);
            cakeEnvironment.SetEnvironmentVariable(name, value);

            var environmentProvider = (IEnvironmentProvider)new EnvironmentProvider(cakeEnvironment);
            var actualValue = environmentProvider.GetEnvironmentVariable(name, defaultValue);

            actualValue.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("MINVERTESTVAR", null, null, null)]
        [InlineData("MINVERTESTVAR", "", null, null)]
        [InlineData("MINVERTESTVAR", " ", null, null)]
        [InlineData("MINVERTESTVAR", null, "1", "1")]
        [InlineData("MINVERTESTVAR", "1",  null, "1")]
        public void Can_Get_Environment_Variable_As_String_With_Overrides(string name, string value, string defaultValue, string expectedValue)
        {
            var cakeEnvironment = new FakeEnvironment(PlatformFamily.Windows, is64Bit: true);
            cakeEnvironment.SetEnvironmentVariable(name, "SOMEVALUE");

            var environmentProvider = (IEnvironmentProvider)new EnvironmentProvider(cakeEnvironment);
            var overrides = new Dictionary<string, string> { {name, value} };
            environmentProvider.SetOverrides(overrides);

            var actualValue = environmentProvider.GetEnvironmentVariable(name, defaultValue);

            actualValue.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("MINVERTESTVAR", null, null, null)]
        [InlineData("MINVERTESTVAR", "", null, null)]
        [InlineData("MINVERTESTVAR", " ", null, null)]
        [InlineData("MINVERTESTVAR", null, true, true)]
        [InlineData("MINVERTESTVAR", "1", false, true)]
        [InlineData("MINVERTESTVAR", "true", false, true)]
        [InlineData("MINVERTESTVAR", "yes", false, true)]
        [InlineData("MINVERTESTVAR", "0", true, false)]
        [InlineData("MINVERTESTVAR", "false", true, false)]
        [InlineData("MINVERTESTVAR", "no", true, false)]
        public void Can_Get_Environment_Variable_As_Bool(string name, string value, bool? defaultValue, bool? expectedValue)
        {
            var cakeEnvironment = new FakeEnvironment(PlatformFamily.Windows, is64Bit: true);
            cakeEnvironment.SetEnvironmentVariable(name, value);

            var environmentProvider = (IEnvironmentProvider)new EnvironmentProvider(cakeEnvironment);
            var actualValue = environmentProvider.GetEnvironmentVariableAsBool(name, defaultValue);

            actualValue.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("MINVERTESTVAR", null, null, null)]
        [InlineData("MINVERTESTVAR", "", null, null)]
        [InlineData("MINVERTESTVAR", " ", null, null)]
        [InlineData("MINVERTESTVAR", null, true, true)]
        [InlineData("MINVERTESTVAR", "1", false, true)]
        [InlineData("MINVERTESTVAR", "true", false, true)]
        [InlineData("MINVERTESTVAR", "yes", false, true)]
        [InlineData("MINVERTESTVAR", "0", true, false)]
        [InlineData("MINVERTESTVAR", "false", true, false)]
        [InlineData("MINVERTESTVAR", "no", true, false)]
        public void Can_Get_Environment_Variable_As_Bool_With_Overrides(string name, string value, bool? defaultValue, bool? expectedValue)
        {
            var cakeEnvironment = new FakeEnvironment(PlatformFamily.Windows, is64Bit: true);
            cakeEnvironment.SetEnvironmentVariable(name, "SOMEVALUE");

            var environmentProvider = (IEnvironmentProvider)new EnvironmentProvider(cakeEnvironment);
            var overrides = new Dictionary<string, string> { {name, value} };
            environmentProvider.SetOverrides(overrides);

            var actualValue = environmentProvider.GetEnvironmentVariableAsBool(name, defaultValue);

            actualValue.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("MINVERTESTVAR", null, null, null)]
        [InlineData("MINVERTESTVAR", "", null, null)]
        [InlineData("MINVERTESTVAR", " ", null, null)]
        [InlineData("MINVERTESTVAR", null, MinVerAutoIncrement.Minor, MinVerAutoIncrement.Minor)]
        [InlineData("MINVERTESTVAR", "Major", null, MinVerAutoIncrement.Major)]
        [InlineData("MINVERTESTVAR", "Minor", null, MinVerAutoIncrement.Minor)]
        [InlineData("MINVERTESTVAR", "Patch", null, MinVerAutoIncrement.Patch)]
        public void Can_Get_Environment_Variable_As_Enum(string name, string value, MinVerAutoIncrement? defaultValue, MinVerAutoIncrement? expectedValue)
        {
            var cakeEnvironment = new FakeEnvironment(PlatformFamily.Windows, is64Bit: true);
            cakeEnvironment.SetEnvironmentVariable(name, value);

            var environmentProvider = (IEnvironmentProvider)new EnvironmentProvider(cakeEnvironment);
            var actualValue = environmentProvider.GetEnvironmentVariableAsEnum(name, defaultValue);

            actualValue.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("MINVERTESTVAR", null, null, null)]
        [InlineData("MINVERTESTVAR", "", null, null)]
        [InlineData("MINVERTESTVAR", " ", null, null)]
        [InlineData("MINVERTESTVAR", null, MinVerAutoIncrement.Minor, MinVerAutoIncrement.Minor)]
        [InlineData("MINVERTESTVAR", "Major", null, MinVerAutoIncrement.Major)]
        [InlineData("MINVERTESTVAR", "Minor", null, MinVerAutoIncrement.Minor)]
        [InlineData("MINVERTESTVAR", "Patch", null, MinVerAutoIncrement.Patch)]
        public void Can_Get_Environment_Variable_As_Enum_With_Overrides(string name, string value, MinVerAutoIncrement? defaultValue, MinVerAutoIncrement? expectedValue)
        {
            var cakeEnvironment = new FakeEnvironment(PlatformFamily.Windows, is64Bit: true);
            cakeEnvironment.SetEnvironmentVariable(name, "SOMEVALUE");

            var environmentProvider = (IEnvironmentProvider)new EnvironmentProvider(cakeEnvironment);
            var overrides = new Dictionary<string, string> { {name, value} };
            environmentProvider.SetOverrides(overrides);

            var actualValue = environmentProvider.GetEnvironmentVariableAsEnum(name, defaultValue);

            actualValue.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("MINVERTESTVAR", null, null, null)]
        [InlineData("MINVERTESTVAR", "", null, null)]
        [InlineData("MINVERTESTVAR", " ", null, null)]
        [InlineData("MINVERTESTVAR", null, "/folder/minver.exe", "/folder/minver.exe")]
        [InlineData("MINVERTESTVAR", "/folder/minver.exe", null, "/folder/minver.exe")]
        public void Can_Get_Environment_Variable_As_FilePath(string name, string value, string defaultValue, string expectedValue)
        {
            var cakeEnvironment = new FakeEnvironment(PlatformFamily.Windows, is64Bit: true);
            cakeEnvironment.SetEnvironmentVariable(name, value);

            var environmentProvider = (IEnvironmentProvider)new EnvironmentProvider(cakeEnvironment);
            var actualValue = environmentProvider.GetEnvironmentVariableAsFilePath(name, defaultValue)?.FullPath;

            actualValue.Should().Be(expectedValue);
        }
    }
}
