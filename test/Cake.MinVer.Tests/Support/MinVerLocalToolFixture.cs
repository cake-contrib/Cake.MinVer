namespace Cake.MinVer.Tests.Support
{
    internal class MinVerLocalToolFixture : MinVerToolFixture<MinVerLocalTool>
    {
        public MinVerLocalToolFixture(string toolFilename = null)
            : base(toolFilename ?? "dotnet.exe")
        {
            Tool = new MinVerLocalTool(FileSystem, Environment, ProcessRunner, Tools, Log);
        }
    }
}
