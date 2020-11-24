using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.MinVer.Tests.Support
{
    internal class MinVerToolFixtureResult : ToolFixtureResult
    {
        public MinVerToolFixtureResult(FilePath path, ProcessSettings process)
            : base(path, process)
        {
        }

        public MinVerVersion Version { get; set; }
    }
}
