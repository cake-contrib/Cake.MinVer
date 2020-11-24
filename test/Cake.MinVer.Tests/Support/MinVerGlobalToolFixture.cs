namespace Cake.MinVer.Tests.Support
{
    internal class MinVerGlobalToolFixture : MinVerToolFixture<MinVerGlobalTool>
    {
        public MinVerGlobalToolFixture(string toolFilename = null)
            : base(toolFilename ?? "minver.exe")
        {
            Tool = new MinVerGlobalTool(FileSystem, Environment, ProcessRunner, Tools, Log);
        }
    }
}
