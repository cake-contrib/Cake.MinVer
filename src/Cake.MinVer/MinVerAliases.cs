using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.MinVer
{
    /// <summary>
    /// MinVer aliases
    /// </summary>
    [CakeAliasCategory("MinVer")]
    [CakeNamespaceImport("Cake.MinVer")]
    public static class MinVerAliases
    {
        /// <summary>
        /// Run the MinVer dotnet tool with default settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///     MinVer();
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("MinVer")]
        public static MinVerVersion MinVer(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.MinVer(new MinVerSettings());
        }

        /// <summary>
        /// Run the MinVer dotnet tool using the settings returned by a configurator.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configurator">The settings configurator.</param>
        /// <example>
        /// <para>Increment the major version (e.g. 'dotnet minver --auto-increment major')</para>
        /// <code>
        /// <![CDATA[
        ///     MinVer(settings => settings.WithAutoIncrement(MinVerAutoIncrement.Major));
        /// ]]>
        /// </code>
        /// <para>Set the build metadata (e.g. 'dotnet minver --build-metadata abcdefg')</para>
        /// <code>
        /// <![CDATA[
        ///     MinVer(settings => settings.WithBuildMetadata("abcdefg"));
        /// ]]>
        /// </code>
        /// <para>Set the default pre-release phase (e.g. 'dotnet minver --default-pre-release-phase preview')</para>
        /// <code>
        /// <![CDATA[
        ///     MinVer(settings => settings.WithDefaultPreReleasePhase("preview"));
        /// ]]>
        /// </code>
        /// <para>Set the minimum major and minor version (e.g. 'dotnet minver --minimum-major-minor 2.5')</para>
        /// <code>
        /// <![CDATA[
        ///     MinVer(settings => settings.WithMinimumMajorMinor("2.5"));
        /// ]]>
        /// </code>
        /// <para>Set the working directory for MinVer to use (e.g. 'dotnet minver --repo C:\MyProject')</para>
        /// <code>
        /// <![CDATA[
        ///     MinVer(settings => settings.WithRepo(@"C:\MyProject"));
        /// ]]>
        /// </code>
        /// <para>Set the tag prefix (e.g. 'dotnet minver --tag-prefix v')</para>
        /// <code>
        /// <![CDATA[
        ///     MinVer(settings => settings.WithTagPrefix("v"));
        /// ]]>
        /// </code>
        /// <para>Set the verbosity (e.g. 'dotnet minver --verbosity trace')</para>
        /// <code>
        /// <![CDATA[
        ///     MinVer(settings => settings.WithVerbosity(MinVerVerbosity.Trace));
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("MinVer")]
        public static MinVerVersion MinVer(this ICakeContext context, Action<MinVerSettings> configurator)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            var settings = new MinVerSettings();
            configurator(settings);
            return context.MinVer(settings);
        }

        /// <summary>
        /// Run the MinVer dotnet tool using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <para>Increment the major version (e.g. 'dotnet minver --auto-increment major')</para>
        /// <code>
        /// <![CDATA[
        ///     var settings = 
        ///         new MinVerSettings 
        ///         {
        ///             AutoIncrement = MinVerAutoIncrement.Major,
        ///         };
        /// 
        ///     MinVer(settings);
        /// ]]>
        /// </code>
        /// <para>Set the build metadata (e.g. 'dotnet minver --build-metadata abcdefg')</para>
        /// <code>
        /// <![CDATA[
        ///     var settings = 
        ///         new MinVerSettings 
        ///         {
        ///             BuildMetadata = "abcdefg",
        ///         };
        /// 
        ///     MinVer(settings);
        /// ]]>
        /// </code>
        /// <para>Set the default pre-release phase (e.g. 'dotnet minver --default-pre-release-phase preview')</para>
        /// <code>
        /// <![CDATA[
        ///     var settings = 
        ///         new MinVerSettings 
        ///         {
        ///             DefaultPreReleasePhase = "preview",
        ///         };
        /// 
        ///     MinVer(settings);
        /// ]]>
        /// </code>
        /// <para>Set the minimum major and minor version (e.g. 'dotnet minver --minimum-major-minor 2.5')</para>
        /// <code>
        /// <![CDATA[
        ///     var settings = 
        ///         new MinVerSettings 
        ///         {
        ///             MinimumMajorMinor = "2.5",
        ///         };
        /// 
        ///     MinVer(settings);
        /// ]]>
        /// </code>
        /// <para>Set the working directory for MinVer to use (e.g. 'dotnet minver --repo C:\MyProject')</para>
        /// <code>
        /// <![CDATA[
        ///     var settings = 
        ///         new MinVerSettings 
        ///         {
        ///             Repo = @"C:\MyProject",
        ///         };
        /// 
        ///     MinVer(settings);
        /// ]]>
        /// </code>
        /// <para>Set the tag prefix (e.g. 'dotnet minver --tag-prefix v')</para>
        /// <code>
        /// <![CDATA[
        ///     var settings = 
        ///         new MinVerSettings 
        ///         {
        ///             TagPrefix = "v",
        ///         };
        /// 
        ///     MinVer(settings);
        /// ]]>
        /// </code>
        /// <para>Set the verbosity (e.g. 'dotnet minver --verbosity trace')</para>
        /// <code>
        /// <![CDATA[
        ///     var settings = 
        ///         new MinVerSettings 
        ///         {
        ///             Verbosity = MinVerVerbosity.Trace,
        ///         };
        /// 
        ///     MinVer(settings);
        /// ]]>
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("MinVer")]
        public static MinVerVersion MinVer(this ICakeContext context, MinVerSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            AddInInformation.LogVersionInformation(context.Log);

            var minVer = new MinVerTool(context.FileSystem, context.Environment, context.ProcessRunner,
                context.Tools, context.Log);

            return minVer.Run(settings);
        }
    }
}
