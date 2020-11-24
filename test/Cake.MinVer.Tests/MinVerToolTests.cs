using Cake.Core;
using Cake.MinVer.Tests.Support;
using Cake.Testing;
using FluentAssertions;
using Xunit;

namespace Cake.MinVer.Tests
{
    public class MinVerToolTests
    {
        [Fact]
        public void Should_Run_Local_Tool_First_By_Default()
        {
            var localToolFixture = new MinVerLocalToolFixture
            {
                StandardOutput = MinVerToolOutputs.OutputWhenTagFoundDefaultVerbosity,
            };

            var globalToolFixture = new MinVerGlobalToolFixture("does-not-exist.exe");

            var fixture = new MinVerToolFixture
            {
                LocalTool = localToolFixture,
                GlobalTool = globalToolFixture,
            };

            var _ = fixture.Run();

            fixture.Result.Version.Should().Be("5.0.1-alpha.0.8");
        }

        [Fact]
        public void Should_Run_Global_Tool_First_If_PreferGlobalTool()
        {
            var localToolFixture = new MinVerLocalToolFixture("does-not-exist.exe");

            var globalToolFixture = new MinVerGlobalToolFixture
            {
                StandardOutput = MinVerToolOutputs.OutputWhenTagFoundVerbosityError,
            };

            var fixture = new MinVerToolFixture
            {
                LocalTool = localToolFixture,
                GlobalTool = globalToolFixture,
                Settings = { PreferGlobalTool = true },
            };

            var _ = fixture.Run();

            fixture.Result.Version.Should().Be("1.2.3-preview.0.4");
        }

        [Fact]
        public void Should_Run_Local_Tool_First_By_Default_And_Fallback_To_Global_Tool()
        {
            var localToolFixture = new MinVerLocalToolFixture();
            localToolFixture.ProcessRunner.Process.SetExitCode(1);

            var globalToolFixture = new MinVerGlobalToolFixture
            {
                StandardOutput = MinVerToolOutputs.OutputWhenTagFoundVerbosityError,
            };

            var fixture = new MinVerToolFixture
            {
                LocalTool = localToolFixture,
                GlobalTool = globalToolFixture,
            };

            var _ = fixture.Run();

            fixture.Result.Version.Should().Be("1.2.3-preview.0.4");
        }

        [Fact]
        public void Should_Run_Global_Tool_First_If_PreferGlobalTool_And_Fallback_To_Global_Tool()
        {
            var localToolFixture = new MinVerLocalToolFixture
            {
                StandardOutput = MinVerToolOutputs.OutputWhenTagFoundDefaultVerbosity,
            };

            var globalToolFixture = new MinVerGlobalToolFixture();
            globalToolFixture.ProcessRunner.Process.SetExitCode(1);

            var fixture = new MinVerToolFixture
            {
                LocalTool = localToolFixture,
                GlobalTool = globalToolFixture,
                Settings = { PreferGlobalTool = true },
            };

            var _ = fixture.Run();

            fixture.Result.Version.Should().Be("5.0.1-alpha.0.8");
        }

        [Fact]
        public void Should_Not_Fallback_If_NoFallback_Is_True()
        {
            var localToolFixture = new MinVerLocalToolFixture();
            localToolFixture.ProcessRunner.Process.SetExitCode(1);

            var globalToolFixture = new MinVerGlobalToolFixture("does-not-exist.exe");

            var fixture = new MinVerToolFixture
            {
                LocalTool = localToolFixture,
                GlobalTool = globalToolFixture,
                Settings = { NoFallback = true },
            };

            fixture.Invoking(f => f.Run())
                .Should().ThrowExactly<CakeException>()
                .And.Message.Should().StartWith("MinVer: Process returned an error (exit code 1)");
        }

        [Fact]
        public void Should_Prefer_Global_Tool_If_ToolPath_Is_Set()
        {
            var localToolFixture = new MinVerLocalToolFixture("does-not-exist.exe");

            var globalToolFixture = new MinVerGlobalToolFixture
            {
                StandardOutput = MinVerToolOutputs.OutputWhenTagFoundVerbosityError,
            };

            var customToolExePath = globalToolFixture.DefaultToolPath.GetDirectory().CombineWithFilePath("customLocation/minver.exe");
            globalToolFixture.FileSystem.CreateFile(customToolExePath);

            var fixture = new MinVerToolFixture
            {
                LocalTool = localToolFixture,
                GlobalTool = globalToolFixture,
                Settings =
                {
                    ToolPath = customToolExePath,
                },
            };

            var _ = fixture.Run();

            fixture.Result.Version.Should().Be("1.2.3-preview.0.4");
        }

        [Fact]
        public void Should_Not_Fallback_If_ToolPath_Is_Set()
        {
            var localToolFixture = new MinVerLocalToolFixture("does-not-exist.exe");

            var globalToolFixture = new MinVerGlobalToolFixture
            {
                StandardOutput = MinVerToolOutputs.OutputWhenTagFoundVerbosityError,
            };

            globalToolFixture.ProcessRunner.Process.SetExitCode(1);

            var customToolExePath = globalToolFixture.DefaultToolPath.GetDirectory().CombineWithFilePath("customLocation/minver.exe");
            globalToolFixture.FileSystem.CreateFile(customToolExePath);

            var fixture = new MinVerToolFixture
            {
                LocalTool = localToolFixture,
                GlobalTool = globalToolFixture,
                Settings =
                {
                    ToolPath = customToolExePath,
                },
            };

            fixture.Invoking(f => f.Run())
                .Should().ThrowExactly<CakeException>()
                .And.Message.Should().StartWith("MinVer: Process returned an error (exit code 1)");
        }
    }
}
