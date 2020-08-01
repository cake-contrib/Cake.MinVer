namespace Cake.MinVer
{
    /// <summary>
    /// --auto-increment &lt;VERSION_PART&gt;
    /// major, minor, or patch (default)
    /// </summary>
    public enum MinVerAutoIncrement
    {
        /// <summary>
        /// Omits the --auto-increment argument (and uses MinVer's default)
        /// </summary>
        Default,

        /// <summary>
        /// --auto-increment major
        /// </summary>
        Major,

        /// <summary>
        /// --auto-increment minor
        /// </summary>
        Minor,

        /// <summary>
        /// --auto-increment patch
        /// </summary>
        Patch,
    }
}
