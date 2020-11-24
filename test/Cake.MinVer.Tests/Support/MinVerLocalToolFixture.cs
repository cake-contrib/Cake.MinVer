using Cake.Core.IO;

namespace Cake.MinVer.Tests.Support
{
    internal class MinVerLocalToolFixture : MinVerToolFixtureBase<MinVerLocalTool>, IMinVerLocalTool
    {
        public MinVerLocalToolFixture(MinVerToolFixture _, MinVerToolContext context)
            : base(context)
        {
            _tool = new MinVerLocalTool(_.FileSystem, _.Environment, ProcessRunner, _.Tools, _.Log);
            StandardOutput = MinVerToolOutputs.DefaultOutputForLocalTool;
        }

        public override FilePath DefaultToolPath => GetDefaultToolPath("dotnet.exe");
    }
}
