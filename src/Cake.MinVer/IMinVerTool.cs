namespace Cake.MinVer
{
    internal interface IMinVerTool
    {
        string ToolName { get; }

        int TryRun(MinVerSettings settings, out MinVerVersion result);
    }
}
