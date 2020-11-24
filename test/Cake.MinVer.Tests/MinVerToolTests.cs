using System;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.MinVer.Tests.Support;
using Cake.Testing;

namespace Cake.MinVer.Tests
{
    public class MinVerToolTests
    {
        private readonly ICakeLog _log;

        public MinVerToolTests(ITestOutputHelper testOutputHelper)
        {
            _log = new XUnitLogger(testOutputHelper);
        }

        [Fact]
        public void Should_Run_Local_Tool_First_By_Default()
        {
            var fixture = new MinVerToolFixture(_log);

            var result = fixture.Run();

            fixture.LocalTool.ShouldHaveRunOnce();
            fixture.GlobalTool.ShouldNotHaveRun();

            result.Version.Should().Be(MinVerToolOutputs.DefaultVersionForLocalTool);
        }

        [Fact]
        public void Should_Run_Global_Tool_First_If_PreferGlobalTool()
        {
            var fixture = new MinVerToolFixture(_log)
            {
                Settings = { PreferGlobalTool = true },
            };

            var result = fixture.Run();

            fixture.GlobalTool.ShouldHaveRunOnce();
            fixture.LocalTool.ShouldNotHaveRun();

            result.Version.Should().Be(MinVerToolOutputs.DefaultVersionForGlobalTool);
        }

        [Fact]
        public void Should_Run_Local_Tool_First_By_Default_And_Fallback_To_Global_Tool()
        {
            var fixture = new MinVerToolFixture(_log);

            fixture.GivenLocalToolFailsToRun();

            var result = fixture.Run();

            fixture.LocalTool.ShouldHaveRunOnce();
            fixture.GlobalTool.ShouldHaveRunOnce();

            fixture.LocalTool.ShouldHaveRunFirst();
            fixture.GlobalTool.ShouldHaveRunSecond();

            result.Version.Should().Be(MinVerToolOutputs.DefaultVersionForGlobalTool);
        }

        [Fact]
        public void Should_Run_Global_Tool_First_If_PreferGlobalTool_And_Fallback_To_Local_Tool()
        {
            var fixture = new MinVerToolFixture(_log)
            {
                Settings = { PreferGlobalTool = true },
            };

            fixture.GivenGlobalToolFailsToRun();

            var result = fixture.Run();

            fixture.GlobalTool.ShouldHaveRunOnce();
            fixture.LocalTool.ShouldHaveRunOnce();

            fixture.GlobalTool.ShouldHaveRunFirst();
            fixture.LocalTool.ShouldHaveRunSecond();

            result.Version.Should().Be(MinVerToolOutputs.DefaultVersionForLocalTool);
        }

        [Fact]
        public void Should_Not_Fallback_If_NoFallback_Is_True()
        {
            var fixture = new MinVerToolFixture(_log)
            {
                Settings = { NoFallback = true },
            };

            fixture.GivenLocalToolFailsToRun();

            fixture.Invoking(f => f.Run())
                .Should().ThrowExactly<CakeException>()
                .And.Message.Should().StartWith("MinVer: Process returned an error (exit code 1)");

            fixture.LocalTool.ShouldHaveRunOnce();
            fixture.GlobalTool.ShouldNotHaveRun();
        }

        [Fact]
        public void Should_Prefer_Global_Tool_using_ToolPath_When_ToolPath_Is_Set()
        {
            var fixture = new MinVerToolFixture(_log);

            fixture.GivenLocalToolIsNotInstalled();
            fixture.GivenGlobalToolIsNotInstalled();

            var customToolExePath = fixture.DefaultToolPath.GetDirectory().CombineWithFilePath("customLocation/custom.exe");
            fixture.FileSystem.CreateFile(customToolExePath);

            fixture.Settings.ToolPath = customToolExePath;

            var result = fixture.Run();

            fixture.GlobalTool.ShouldHaveRunOnce();
            fixture.LocalTool.ShouldNotHaveRun();

            result.Version.Should().Be(MinVerToolOutputs.DefaultVersionForGlobalTool);
        }

        [Fact]
        public void Should_Not_Fallback_If_ToolPath_Is_Set()
        {
            var fixture = new MinVerToolFixture(_log);

            fixture.GivenLocalToolIsNotInstalled();
            fixture.GivenGlobalToolIsNotInstalled();

            var customToolExePath = fixture.DefaultToolPath.GetDirectory().CombineWithFilePath("customLocation/custom.exe");
            fixture.FileSystem.CreateFile(customToolExePath);

            fixture.Settings.ToolPath = customToolExePath;

            fixture.GivenGlobalToolFailsToRun();

            fixture.Invoking(f => f.Run())
                .Should().ThrowExactly<CakeException>()
                .And.Message.Should().StartWith("MinVer: Process returned an error (exit code 1)");

            fixture.GlobalTool.ShouldHaveRunOnce();
            fixture.LocalTool.ShouldNotHaveRun();
        }

        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            var fixture = new MinVerToolFixture(_log)
            {
                Settings = null,
            };

            fixture.GivenDefaultToolDoNotExist();

            fixture.Invoking(f => f.Run())
                .Should().ThrowExactly<ArgumentNullException>()
                .And.ParamName.Should().Be("settings");
        }

        [Fact]
        public void Should_Run_Local_Tool_By_Running_dotnet_minver()
        {
            var fixture = new MinVerToolFixture(_log);
            var result = fixture.Run();

            result.Path.FullPath.Should().Be(fixture.LocalTool.DefaultToolPath.FullPath);
            result.Args.Should().Be("minver");
        }

        [Fact]
        public void Should_Run_Global_Tool_By_Running_minver()
        {
            var fixture = new MinVerToolFixture(_log)
            {
                Settings = {PreferGlobalTool = true},
            };

            var result = fixture.Run();

            result.Path.FullPath.Should().Be(fixture.GlobalTool.DefaultToolPath.FullPath);
            result.Args.Should().BeEmpty();
        }

        [Theory]
        [InlineData(null, "minver")]
        [InlineData(MinVerAutoIncrement.Major, "minver --auto-increment major")]
        [InlineData(MinVerAutoIncrement.Minor, "minver --auto-increment minor")]
        [InlineData(MinVerAutoIncrement.Patch, "minver --auto-increment patch")]
        public void Should_Add_Auto_Increment_To_Arguments(MinVerAutoIncrement? autoIncrement, string expectedArgs)
        {
            var fixture = new MinVerToolFixture(_log)
            {
                Settings = { AutoIncrement = autoIncrement },
            };

            var result = fixture.Run();

            result.Args.Should().Be(expectedArgs);
        }

        [Fact]
        public void Should_Add_Build_Metadata_Arguments()
        {
            var fixture = new MinVerToolFixture(_log)
            {
                Settings = { BuildMetadata = "1234abc" },
            };

            var result = fixture.Run();

            result.Args.Should().Be("minver --build-metadata \"1234abc\"");
        }

        [Fact]
        public void Should_Add_Default_Pre_Release_Phase_Arguments()
        {
            var fixture = new MinVerToolFixture(_log)
            {
                Settings = { DefaultPreReleasePhase = "preview" },
            };

            var result = fixture.Run();

            result.Args.Should().Be("minver --default-pre-release-phase \"preview\"");
        }

        [Fact]
        public void Should_Add_Minimum_Major_Minor_Arguments()
        {
            var fixture = new MinVerToolFixture(_log)
            {
                Settings = { MinimumMajorMinor = "2.0" },
            };

            var result = fixture.Run();

            result.Args.Should().Be("minver --minimum-major-minor \"2.0\"");
        }

        [Fact]
        public void Should_Add_Repo_Arguments()
        {
            var fixture = new MinVerToolFixture(_log)
            {
                Settings = { Repo = "./src" },
            };

            var result = fixture.Run();

            result.Args.Should().Be("minver --repo \"src\"");
        }

        [Fact]
        public void Should_Add_Tag_Prefix_Arguments()
        {
            var fixture = new MinVerToolFixture(_log)
            {
                Settings = { TagPrefix = "v" },
            };

            var result = fixture.Run();

            result.Args.Should().Be("minver --tag-prefix \"v\"");
        }

        [Theory]
        [InlineData(null, "minver")]
        [InlineData(MinVerVerbosity.Error, "minver --verbosity error")]
        [InlineData(MinVerVerbosity.Warn, "minver --verbosity warn")]
        [InlineData(MinVerVerbosity.Info, "minver --verbosity info")]
        [InlineData(MinVerVerbosity.Debug, "minver --verbosity debug")]
        [InlineData(MinVerVerbosity.Trace, "minver --verbosity trace")]
        public void Should_Add_Verbosity_To_Arguments(MinVerVerbosity? verbosity, string expectedArgs)
        {
            var fixture = new MinVerToolFixture(_log)
            {
                Settings = { Verbosity = verbosity },
            };

            var result = fixture.Run();

            result.Args.Should().Be(expectedArgs);
        }

        [Fact]
        public void Should_Throw_Cake_Exception_If_Cant_Parse_MinVer_Version()
        {
            var fixture = new MinVerToolFixture(_log);

            fixture.LocalTool.StandardOutput = new[] { "abcd" };

            fixture.Invoking(f => f.Run())
                .Should().ThrowExactly<CakeException>()
                .WithMessage("Version 'abcd' is not valid.");
        }

        [Fact]
        public void Should_Return_MinVer_Calculated_Version_When_Not_A_Git_Repository()
        {
            var fixture = new MinVerToolFixture(_log);

            fixture.LocalTool.StandardOutput = MinVerToolOutputs.OutputWhenNotAGitRepo;

            var result = fixture.Run();

            result.Version.ToString().Should().Be("0.0.0-alpha.0");
        }

        [Fact]
        public void Should_Return_MinVer_Calculated_Version_When_Tag_Not_Found()
        {
            var fixture = new MinVerToolFixture(_log);

            fixture.LocalTool.StandardOutput = MinVerToolOutputs.OutputWhenTagNotFound;

            var result = fixture.Run();

            result.Version.ToString().Should().Be("0.0.0-alpha.0.42");
        }

        [Fact]
        public void Should_Return_MinVer_Calculated_Version_When_Tag_Found_Default_Verbosity()
        {
            var fixture = new MinVerToolFixture(_log);

            fixture.LocalTool.StandardOutput = MinVerToolOutputs.OutputWhenTagFoundDefaultVerbosity;

            var result = fixture.Run();

            result.Version.ToString().Should().Be("5.0.1-alpha.0.8");
        }

        [Fact]
        public void Should_Return_MinVer_Calculated_Version_When_Tag_Found_Verbosity_Error()
        {
            var fixture = new MinVerToolFixture(_log);

            fixture.LocalTool.StandardOutput = MinVerToolOutputs.OutputWhenTagFoundVerbosityError;

            var result = fixture.Run();

            result.Version.ToString().Should().Be("1.2.3-preview.0.4");
        }

        [Theory]
        [InlineData(MinVerAutoIncrement.Minor, null, null, MinVerAutoIncrement.Minor)]
        [InlineData(null, MinVerAutoIncrement.Minor, null, MinVerAutoIncrement.Minor)]
        [InlineData(null, null, MinVerAutoIncrement.Minor, MinVerAutoIncrement.Minor)]
        [InlineData(MinVerAutoIncrement.Minor, MinVerAutoIncrement.Major, MinVerAutoIncrement.Patch, MinVerAutoIncrement.Minor)]
        [InlineData(null, MinVerAutoIncrement.Minor, MinVerAutoIncrement.Patch, MinVerAutoIncrement.Minor)]
        public void Should_Add_Auto_Increment_Arguments_From_Environment_Variable(MinVerAutoIncrement? argValue, MinVerAutoIncrement? envVarOverrideValue, MinVerAutoIncrement? envVarValue, MinVerAutoIncrement expected)
        {
            var fixture = new MinVerToolFixture(_log)
            {
                Settings = { AutoIncrement = argValue },
            };

            if (envVarOverrideValue.HasValue)
            {
                fixture.Settings.EnvironmentVariables[MinVerEnvironmentVariables.MINVERAUTOINCREMENT] =
                    envVarOverrideValue.Value.ToString().ToLowerInvariant();
            }

            if (envVarValue.HasValue)
            {
                fixture.Environment.SetEnvironmentVariable(MinVerEnvironmentVariables.MINVERAUTOINCREMENT,
                    envVarValue.Value.ToString().ToLowerInvariant());
            }

            var result = fixture.Run();

            result.Args.Should().Be($"minver --auto-increment {expected.ToString().ToLowerInvariant()}");
        }

        [Theory]
        [InlineData("1234abc", null, null, "1234abc")]
        [InlineData(null, "1234abc", null, "1234abc")]
        [InlineData(null, null, "1234abc", "1234abc")]
        [InlineData("1234abc", "x", "y", "1234abc")]
        [InlineData(null, "1234abc", "y", "1234abc")]
        public void Should_Add_Build_Metadata_Arguments_From_Environment_Variable(string argValue, string envVarOverrideValue, string envVarValue, string expected)
        {
            var fixture = new MinVerToolFixture(_log)
            {
                Settings = { BuildMetadata = argValue },
            };

            if (!string.IsNullOrWhiteSpace(envVarOverrideValue))
            {
                fixture.Settings.EnvironmentVariables[MinVerEnvironmentVariables.MINVERBUILDMETADATA] = envVarOverrideValue;
            }

            if (!string.IsNullOrWhiteSpace(envVarValue))
            {
                fixture.Environment.SetEnvironmentVariable(MinVerEnvironmentVariables.MINVERBUILDMETADATA, envVarValue);
            }

            var result = fixture.Run();

            result.Args.Should().Be($"minver --build-metadata \"{expected}\"");
        }

        [Theory]
        [InlineData("preview", null, null, "preview")]
        [InlineData(null, "preview", null, "preview")]
        [InlineData(null, null, "preview", "preview")]
        [InlineData("preview", "x", "y", "preview")]
        [InlineData(null, "preview", "y", "preview")]
        public void Should_Add_Default_PreRelease_Phase_Arguments_From_Environment_Variable(string argValue, string envVarOverrideValue, string envVarValue, string expected)
        {
            var fixture = new MinVerToolFixture(_log)
            {
                Settings = { DefaultPreReleasePhase = argValue },
            };

            if (!string.IsNullOrWhiteSpace(envVarOverrideValue))
            {
                fixture.Settings.EnvironmentVariables[MinVerEnvironmentVariables.MINVERDEFAULTPRERELEASEPHASE] = envVarOverrideValue;
            }

            if (!string.IsNullOrWhiteSpace(envVarValue))
            {
                fixture.Environment.SetEnvironmentVariable(MinVerEnvironmentVariables.MINVERDEFAULTPRERELEASEPHASE, envVarValue);
            }

            var result = fixture.Run();

            result.Args.Should().Be($"minver --default-pre-release-phase \"{expected}\"");
        }

        [Theory]
        [InlineData("2.5", null, null, "2.5")]
        [InlineData(null, "2.5", null, "2.5")]
        [InlineData(null, null, "2.5", "2.5")]
        [InlineData("2.5", "x", "y", "2.5")]
        [InlineData(null, "2.5", "y", "2.5")]
        public void Should_Add_Minimum_Major_Minor_Arguments_From_Environment_Variable(string argValue, string envVarOverrideValue, string envVarValue, string expected)
        {
            var fixture = new MinVerToolFixture(_log)
            {
                Settings = { MinimumMajorMinor = argValue },
            };

            if (!string.IsNullOrWhiteSpace(envVarOverrideValue))
            {
                fixture.Settings.EnvironmentVariables[MinVerEnvironmentVariables.MINVERMINIMUMMAJORMINOR] = envVarOverrideValue;
            }

            if (!string.IsNullOrWhiteSpace(envVarValue))
            {
                fixture.Environment.SetEnvironmentVariable(MinVerEnvironmentVariables.MINVERMINIMUMMAJORMINOR, envVarValue);
            }

            var result = fixture.Run();

            result.Args.Should().Be($"minver --minimum-major-minor \"{expected}\"");
        }

        [Theory]
        [InlineData("v", null, null, "v")]
        [InlineData(null, "v", null, "v")]
        [InlineData(null, null, "v", "v")]
        [InlineData("v", "x", "y", "v")]
        [InlineData(null, "v", "y", "v")]
        public void Should_Add_Tag_Prefix_Arguments_From_Environment_Variable(string argValue, string envVarOverrideValue, string envVarValue, string expected)
        {
            var fixture = new MinVerToolFixture(_log)
            {
                Settings = { TagPrefix = argValue },
            };

            if (!string.IsNullOrWhiteSpace(envVarOverrideValue))
            {
                fixture.Settings.EnvironmentVariables[MinVerEnvironmentVariables.MINVERTAGPREFIX] = envVarOverrideValue;
            }

            if (!string.IsNullOrWhiteSpace(envVarValue))
            {
                fixture.Environment.SetEnvironmentVariable(MinVerEnvironmentVariables.MINVERTAGPREFIX, envVarValue);
            }

            var result = fixture.Run();

            result.Args.Should().Be($"minver --tag-prefix \"{expected}\"");
        }

        [Theory]
        [InlineData(MinVerVerbosity.Trace, null, null, MinVerVerbosity.Trace)]
        [InlineData(null, MinVerVerbosity.Trace, null, MinVerVerbosity.Trace)]
        [InlineData(null, null, MinVerVerbosity.Trace, MinVerVerbosity.Trace)]
        [InlineData(MinVerVerbosity.Trace, MinVerVerbosity.Error, MinVerVerbosity.Error, MinVerVerbosity.Trace)]
        [InlineData(null, MinVerVerbosity.Trace, MinVerVerbosity.Warn, MinVerVerbosity.Trace)]
        public void Should_Add_Verbosity_Arguments_From_Environment_Variable(MinVerVerbosity? argValue, MinVerVerbosity? envVarOverrideValue, MinVerVerbosity? envVarValue, MinVerVerbosity expected)
        {
            var fixture = new MinVerToolFixture(_log)
            {
                Settings = { Verbosity = argValue },
            };

            if (envVarOverrideValue.HasValue)
            {
                fixture.Settings.EnvironmentVariables[MinVerEnvironmentVariables.MINVERVERBOSITY] =
                    envVarOverrideValue.Value.ToString().ToLowerInvariant();
            }

            if (envVarValue.HasValue)
            {
                fixture.Environment.SetEnvironmentVariable(MinVerEnvironmentVariables.MINVERVERBOSITY,
                    envVarValue.Value.ToString().ToLowerInvariant());
            }

            var result = fixture.Run();

            result.Args.Should().Be($"minver --verbosity {expected.ToString().ToLowerInvariant()}");
        }
    }
}
