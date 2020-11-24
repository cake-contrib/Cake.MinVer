using Cake.Core.IO;

namespace Cake.MinVer.Tests.Support
{
    internal class MinVerGlobalToolFixture : MinVerToolFixtureBase<MinVerGlobalTool>, IMinVerGlobalTool
    {
        public MinVerGlobalToolFixture(MinVerToolFixture _, MinVerToolContext context)
            : base(context)
        {
            _tool = new MinVerGlobalTool(_.FileSystem, _.Environment, ProcessRunner, _.Tools, _.Log);
            StandardOutput = MinVerToolOutputs.DefaultOutputForGlobalTool;
        }

        public override FilePath DefaultToolPath => GetDefaultToolPath("minver.exe");
    }
}
