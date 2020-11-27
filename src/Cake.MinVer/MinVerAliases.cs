// Copyright 2020 C. Augusto Proiete & Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
        /// var buildVersion = MinVer();
        /// 
        /// Information($"Version: {buildVersion.Version}");
        /// Information($"Major: {buildVersion.Major}");
        /// Information($"Minor: {buildVersion.Minor}");
        /// Information($"Patch: {buildVersion.Patch}");
        /// Information($"PreRelease: {buildVersion.PreRelease}");
        /// Information($"BuildMetadata: {buildVersion.BuildMetadata}");
        /// // ...
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
        /// var buildVersion = MinVer(settings => settings.WithAutoIncrement(MinVerAutoIncrement.Major));
        /// Information($"Version: {buildVersion.Version}");
        /// // ...
        /// ]]>
        /// </code>
        /// <para>Set the build metadata (e.g. 'dotnet minver --build-metadata abcdefg')</para>
        /// <code>
        /// <![CDATA[
        /// var buildVersion = MinVer(settings => settings.WithBuildMetadata("abcdefg"));
        /// Information($"Version: {buildVersion.Version}");
        /// // ...
        /// ]]>
        /// </code>
        /// <para>Set the default pre-release phase (e.g. 'dotnet minver --default-pre-release-phase preview')</para>
        /// <code>
        /// <![CDATA[
        /// var buildVersion = MinVer(settings => settings.WithDefaultPreReleasePhase("preview"));
        /// Information($"Version: {buildVersion.Version}");
        /// // ...
        /// ]]>
        /// </code>
        /// <para>Set the minimum major and minor version (e.g. 'dotnet minver --minimum-major-minor 2.5')</para>
        /// <code>
        /// <![CDATA[
        /// var buildVersion = MinVer(settings => settings.WithMinimumMajorMinor("2.5"));
        /// Information($"Version: {buildVersion.Version}");
        /// // ...
        /// ]]>
        /// </code>
        /// <para>Set the working directory for MinVer to use (e.g. 'dotnet minver --repo C:\MyProject')</para>
        /// <code>
        /// <![CDATA[
        /// var buildVersion = MinVer(settings => settings.WithRepo(@"C:\MyProject"));
        /// Information($"Version: {buildVersion.Version}");
        /// // ...
        /// ]]>
        /// </code>
        /// <para>Set the tag prefix (e.g. 'dotnet minver --tag-prefix v')</para>
        /// <code>
        /// <![CDATA[
        /// var buildVersion = MinVer(settings => settings.WithTagPrefix("v"));
        /// Information($"Version: {buildVersion.Version}");
        /// // ...
        /// ]]>
        /// </code>
        /// <para>Run MinVer as a global tool (e.g. 'minver'), instead of local tool (e.g. 'dotnet minver')</para>
        /// <code>
        /// <![CDATA[
        /// var buildVersion = MinVer(settings => settings.WithPreferGlobalTool());
        /// Information($"Version: {buildVersion.Version}");
        /// // ...
        /// ]]>
        /// </code>
        /// <para>Disable the automatic fallback to global tool (or local tool) in case of errors</para>
        /// <code>
        /// <![CDATA[
        /// var buildVersion = MinVer(settings => settings.WithNoFallback());
        /// Information($"Version: {buildVersion.Version}");
        /// // ...
        /// ]]>
        /// </code>
        /// <para>Set the verbosity (e.g. 'dotnet minver --verbosity trace')</para>
        /// <code>
        /// <![CDATA[
        /// var buildVersion = MinVer(settings => settings.WithVerbosity(MinVerVerbosity.Trace));
        /// Information($"Version: {buildVersion.Version}");
        /// // ...
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
        /// var settings = new MinVerSettings
        /// {
        ///     AutoIncrement = MinVerAutoIncrement.Major,
        /// };
        /// 
        /// var buildVersion = MinVer(settings);
        /// Information($"Version: {buildVersion.Version}");
        /// // ...
        /// ]]>
        /// </code>
        /// <para>Set the build metadata (e.g. 'dotnet minver --build-metadata abcdefg')</para>
        /// <code>
        /// <![CDATA[
        /// var settings = new MinVerSettings
        /// {
        ///     BuildMetadata = "abcdefg",
        /// };
        /// 
        /// var buildVersion = MinVer(settings);
        /// Information($"Version: {buildVersion.Version}");
        /// // ...
        /// ]]>
        /// </code>
        /// <para>Set the default pre-release phase (e.g. 'dotnet minver --default-pre-release-phase preview')</para>
        /// <code>
        /// <![CDATA[
        /// var settings = new MinVerSettings
        /// {
        ///     DefaultPreReleasePhase = "preview",
        /// };
        /// 
        /// var buildVersion = MinVer(settings);
        /// Information($"Version: {buildVersion.Version}");
        /// // ...
        /// ]]>
        /// </code>
        /// <para>Set the minimum major and minor version (e.g. 'dotnet minver --minimum-major-minor 2.5')</para>
        /// <code>
        /// <![CDATA[
        /// var settings = new MinVerSettings
        /// {
        ///     MinimumMajorMinor = "2.5",
        /// };
        /// 
        /// var buildVersion = MinVer(settings);
        /// Information($"Version: {buildVersion.Version}");
        /// // ...
        /// ]]>
        /// </code>
        /// <para>Set the working directory for MinVer to use (e.g. 'dotnet minver --repo C:\MyProject')</para>
        /// <code>
        /// <![CDATA[
        /// var settings = new MinVerSettings
        /// {
        ///     Repo = @"C:\MyProject",
        /// };
        /// 
        /// var buildVersion = MinVer(settings);
        /// Information($"Version: {buildVersion.Version}");
        /// // ...
        /// ]]>
        /// </code>
        /// <para>Set the tag prefix (e.g. 'dotnet minver --tag-prefix v')</para>
        /// <code>
        /// <![CDATA[
        /// var settings = new MinVerSettings
        /// {
        ///     TagPrefix = "v",
        /// };
        /// 
        /// var buildVersion = MinVer(settings);
        /// Information($"Version: {buildVersion.Version}");
        /// // ...
        /// ]]>
        /// </code>
        /// <para>Run MinVer as a global tool (e.g. 'minver'), instead of local tool (e.g. 'dotnet minver')</para>
        /// <code>
        /// <![CDATA[
        /// var settings = new MinVerSettings
        /// {
        ///     PreferGlobalTool = true,
        /// };
        /// 
        /// var buildVersion = MinVer(settings);
        /// Information($"Version: {buildVersion.Version}");
        /// // ...
        /// ]]>
        /// </code>
        /// <para>Disable the automatic fallback to global tool (or local tool) in case of errors</para>
        /// <code>
        /// <![CDATA[
        /// var settings = new MinVerSettings
        /// {
        ///     NoFallback = true,
        /// };
        /// 
        /// var buildVersion = MinVer(settings);
        /// Information($"Version: {buildVersion.Version}");
        /// // ...
        /// ]]>
        /// </code>
        /// <para>Set the verbosity (e.g. 'dotnet minver --verbosity trace')</para>
        /// <code>
        /// <![CDATA[
        /// var settings = new MinVerSettings
        /// {
        ///     Verbosity = MinVerVerbosity.Trace,
        /// };
        /// 
        /// var buildVersion = MinVer(settings);
        /// Information($"Version: {buildVersion.Version}");
        /// // ...
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
