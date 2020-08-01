namespace Cake.MinVer
{
    /// <summary>
    /// --verbosity &lt;VERBOSITY&gt;
    /// error, warn, info (default), debug, or trace
    /// </summary>
    public enum MinVerVerbosity
    {
        /// <summary>
        /// Omits the --verbosity argument (and uses MinVer's default)
        /// </summary>
        Default,

        /// <summary>
        /// --verbosity error
        /// </summary>
        Error,

        /// <summary>
        /// --verbosity warn
        /// </summary>
        Warn,

        /// <summary>
        /// --verbosity info
        /// </summary>
        Info,

        /// <summary>
        /// --verbosity debug
        /// </summary>
        Debug,

        /// <summary>
        /// --verbosity trace
        /// </summary>
        Trace,
    }
}
