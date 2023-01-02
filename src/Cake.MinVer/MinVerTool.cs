#region Copyright 2020-2023 C. Augusto Proiete & Contributors
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
using System.Globalization;
using Cake.Common.Tools.DotNet;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.MinVer.Utils;

namespace Cake.MinVer;

/// <summary>
/// MinVer dotnet tool.
/// </summary>
public class MinVerTool : DotNetTool<MinVerSettings>
{
    private readonly IMinVerLocalTool _localTool;
    private readonly IMinVerGlobalTool _globalTool;
    private readonly IEnvironmentProvider _environmentProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="MinVerTool" /> class.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="processRunner">The process runner.</param>
    /// <param name="tools">The tool locator.</param>
    /// <param name="log">Cake log instance.</param>
    public MinVerTool(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner,
        IToolLocator tools, ICakeLog log)
        : this(fileSystem, environment, processRunner, tools, log, localTool: null, globalTool: null)
    {
    }

    internal MinVerTool(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner,
        IToolLocator tools, ICakeLog log, IMinVerLocalTool localTool, IMinVerGlobalTool globalTool)
        : base(fileSystem, environment, processRunner, tools)
    {
        _environmentProvider = new EnvironmentProvider(environment);

        CakeLog = log ?? throw new ArgumentNullException(nameof(log));

        _localTool = localTool ?? new MinVerLocalTool(fileSystem, environment, processRunner, tools, log);
        _globalTool = globalTool ?? new MinVerGlobalTool(fileSystem, environment, processRunner, tools, log);
    }

    /// <summary>
    /// Cake log instance.
    /// </summary>
    public ICakeLog CakeLog { get; }

    /// <summary>
    /// Run the MinVer dotnet tool using the specified settings.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <returns>The MinVer calculated version information</returns>
    public MinVerVersion Run(MinVerSettings settings)
    {
        CakeLog.Verbose("Executing {0} tool", GetToolName());

        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        _environmentProvider.SetOverrides(settings.EnvironmentVariables);

        var finalSettings = CloneSettingsAndApplyEnvVariables(settings);

        IMinVerTool preferredTool;
        IMinVerTool fallbackTool;

        if (finalSettings.PreferGlobalTool.GetValueOrDefault())
        {
            preferredTool = _globalTool;
            fallbackTool = _localTool;
        }
        else
        {
            preferredTool = _localTool;
            fallbackTool = _globalTool;
        }

        var preferredToolExitCode = preferredTool.TryRun(finalSettings, out var minVerVersion);
        if (preferredToolExitCode == 0)
        {
            return minVerVersion;
        }

        if (finalSettings.NoFallback.GetValueOrDefault())
        {
            ProcessExitCode(preferredToolExitCode);
            return null;
        }

        var fallbackToolExitCode = fallbackTool.TryRun(finalSettings, out minVerVersion);
        if (fallbackToolExitCode == 0)
        {
            CakeLog.Verbose(string.Format(CultureInfo.InvariantCulture,
                "{0}: Process returned an error (exit code {1}), but {2} executed successfully.",
                preferredTool.ToolName, preferredToolExitCode, fallbackTool.ToolName));

            ProcessExitCode(fallbackToolExitCode);
            return minVerVersion;
        }

        CakeLog.Verbose(string.Format(CultureInfo.InvariantCulture,
            "{0}: Process returned an error (exit code {1}).", preferredTool.ToolName, preferredToolExitCode));

        CakeLog.Verbose(string.Format(CultureInfo.InvariantCulture,
            "{0}: Process returned an error (exit code {1}).", fallbackTool.ToolName, fallbackToolExitCode));

        ProcessExitCode(preferredToolExitCode);
        return null;
    }

    /// <inheritdoc />
    protected override string GetToolName()
    {
        return "MinVer";
    }

    private MinVerSettings CloneSettingsAndApplyEnvVariables(MinVerSettings settings)
    {
        var finalSettings = settings.Clone();

        finalSettings.AutoIncrement = settings.AutoIncrement ?? _environmentProvider
            .GetEnvironmentVariableAsEnum<MinVerAutoIncrement>(MinVerEnvironmentVariables.MINVERAUTOINCREMENT);

        if (string.IsNullOrWhiteSpace(finalSettings.BuildMetadata))
        {
            finalSettings.BuildMetadata = _environmentProvider
                .GetEnvironmentVariable(MinVerEnvironmentVariables.MINVERBUILDMETADATA);
        }

        if (string.IsNullOrWhiteSpace(finalSettings.DefaultPreReleasePhase))
        {
            finalSettings.DefaultPreReleasePhase = _environmentProvider
                .GetEnvironmentVariable(MinVerEnvironmentVariables.MINVERDEFAULTPRERELEASEPHASE);
        }

        if (string.IsNullOrWhiteSpace(finalSettings.MinimumMajorMinor))
        {
            finalSettings.MinimumMajorMinor = _environmentProvider
                .GetEnvironmentVariable(MinVerEnvironmentVariables.MINVERMINIMUMMAJORMINOR);
        }

        if (string.IsNullOrWhiteSpace(finalSettings.TagPrefix))
        {
            finalSettings.TagPrefix = _environmentProvider
                .GetEnvironmentVariable(MinVerEnvironmentVariables.MINVERTAGPREFIX);
        }

        finalSettings.PreferGlobalTool ??= _environmentProvider
            .GetEnvironmentVariableAsBool(MinVerEnvironmentVariables.MINVERPREFERGLOBALTOOL);

        finalSettings.NoFallback ??= _environmentProvider
            .GetEnvironmentVariableAsBool(MinVerEnvironmentVariables.MINVERNOFALLBACK);

        finalSettings.Verbosity ??= _environmentProvider
            .GetEnvironmentVariableAsEnum<MinVerVerbosity>(MinVerEnvironmentVariables.MINVERVERBOSITY);

        finalSettings.ToolPath ??=  _environmentProvider
            .GetEnvironmentVariableAsFilePath(MinVerEnvironmentVariables.MINVERTOOLPATH);

        if (!(finalSettings.ToolPath is null))
        {
            // If ToolPath is specified, it means it's a global tool in a custom location (also known as a tool-path tool)
            // https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools
            finalSettings.PreferGlobalTool = true;

            // If ToolPath is specified, we try to run that specific tool only... No fallback
            finalSettings.NoFallback = true;
        }

        return finalSettings;
    }
}
