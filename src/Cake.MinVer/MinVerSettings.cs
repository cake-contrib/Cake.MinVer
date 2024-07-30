#region Copyright 2020-2024 C. Augusto Proiete & Contributors
//
// Licensed under the MIT (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://opensource.org/licenses/MIT
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using Cake.Common.Tools.DotNet;
using Cake.Core.IO;

namespace Cake.MinVer;

/// <summary>
/// Contains settings used by <see cref="MinVerTool" />.
/// </summary>
public class MinVerSettings : DotNetSettings, ICloneable
{
    /// <summary>
    /// Set the version part to be automatically incremented.
    /// --auto-increment &lt;VERSION_PART&gt;
    /// major, minor, or patch (default)
    /// </summary>
    public MinVerAutoIncrement? AutoIncrement { get; set; }

    /// <summary>
    /// Set the build metadata.
    /// --build-metadata &lt;BUILD_METADATA&gt;
    /// </summary>
    public string BuildMetadata { get; set; }

    /// <summary>
    /// Set the default pre-release phase.
    /// --default-pre-release-phase &lt;PHASE&gt;
    /// alpha (default), preview, etc.
    /// </summary>
    public string DefaultPreReleasePhase { get; set; }

    /// <summary>
    /// Set the minimum major and minor version.
    /// --minimum-major-minor &lt;MINIMUM_MAJOR_MINOR&gt;
    /// 1.0, 1.1, 2.0, etc.
    /// </summary>
    public string MinimumMajorMinor { get; set; }

    /// <summary>
    /// Set the working directory for MinVer to use.
    /// --repo &lt;REPO&gt;
    /// </summary>
    public DirectoryPath Repo { get; set; }

    /// <summary>
    /// Set the tag prefix.
    /// --tag-prefix &lt;TAG_PREFIX&gt;
    /// </summary>
    public string TagPrefix { get; set; }

    /// <summary>
    /// By default, MinVer is executed as a local tool first and, in case of error, fallback(*) to global tool
    /// Set <see cref="PreferGlobalTool" /> to <see langword="true" /> to execute MinVer as global tool first and,
    /// in case of an error, fallback(*) to local tool
    /// 
    /// (*) Unless the fallback is disabled via <see cref="NoFallback" />
    /// 
    /// Local tool = `dotnet minver`
    /// Global tool = `minver`
    /// </summary>
    public bool? PreferGlobalTool { get; set; }

    /// <summary>
    /// By default, MinVer is executed as a local tool first(*) and, in case of error, fallback to global tool(*)
    /// Set <see cref="NoFallback" /> to <see langword="true" /> to disable the fallback in case of an error
    /// 
    /// (*) Unless <see cref="PreferGlobalTool" /> is set to <see langword="true" />, in which case MinVer is
    /// executed as a global tool first and, in case of an error, fallback to local tool
    /// 
    /// Local tool = `dotnet minver`
    /// Global tool = `minver`
    /// </summary>
    public bool? NoFallback { get; set; }

    /// <summary>
    /// Set the verbosity.
    /// --verbosity &lt;VERBOSITY&gt;
    /// error, warn, info (default), debug, or trace
    /// </summary>
    public new MinVerVerbosity? Verbosity { get; set; }

    internal DotNetVerbosity? ToolVerbosity
    {
        get => base.Verbosity;
        set => base.Verbosity = value;
    }

    /// <summary>
    /// Creates a shallow clone of this <see cref="MinVerSettings" /> instance
    /// </summary>
    /// <returns></returns>
    public MinVerSettings Clone()
    {
        var clone = new MinVerSettings
        {
            AutoIncrement = AutoIncrement,
            BuildMetadata = BuildMetadata,
            DefaultPreReleasePhase = DefaultPreReleasePhase,
            MinimumMajorMinor = MinimumMajorMinor,
            Repo = Repo,
            TagPrefix = TagPrefix,
            PreferGlobalTool = PreferGlobalTool,
            NoFallback = NoFallback,
            Verbosity = Verbosity,
            ToolVerbosity = ToolVerbosity,
            
            DiagnosticOutput = DiagnosticOutput,
            ToolPath = ToolPath,
            ToolTimeout = ToolTimeout,
            WorkingDirectory = WorkingDirectory,
            NoWorkingDirectory = NoWorkingDirectory,
            ArgumentCustomization = ArgumentCustomization,
            EnvironmentVariables = EnvironmentVariables,
            HandleExitCode = HandleExitCode,
        };

        return clone;
    }

    object ICloneable.Clone()
    {
        return Clone();
    }
}
