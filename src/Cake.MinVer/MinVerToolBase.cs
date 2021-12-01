#region Copyright 2020-2021 C. Augusto Proiete & Contributors
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
using System.Collections.Generic;
using System.Linq;
using Cake.Common.Tools.DotNet;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.MinVer
{
    internal abstract class MinVerToolBase : DotNetTool<MinVerSettings>, IMinVerTool
    {
        protected MinVerToolBase(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner,
            IToolLocator tools, ICakeLog log)
            : base(fileSystem, environment, processRunner, tools)
        {
            CakeLog = log ?? throw new ArgumentNullException(nameof(log));
        }

        public ICakeLog CakeLog { get; }

        public string ToolName => GetToolName();

        public int TryRun(MinVerSettings settings, out MinVerVersion version)
        {
            if (settings is null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            string[] standardOutput = null;
            MinVerVersion minVerVersion = null;
            int? exitCode = 1;
            var minVerArgs = GetArguments(settings);

            var processSettings = new ProcessSettings
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Silent = true,
                RedirectedStandardOutputHandler = output =>
                {
                    if (!(output is null))
                    {
                        CakeLog.Verbose(output);
                    }

                    return output;
                },
                RedirectedStandardErrorHandler = error =>
                {
                    if (!(error is null))
                    {
                        CakeLog.Verbose(error);
                    }

                    return error;
                },
            };

            try
            {
                Run(settings, minVerArgs, processSettings, p =>
                {
                    exitCode = p.GetExitCode();
                    if (exitCode == 0)
                    {
                        standardOutput = p.GetStandardOutput()?.ToArray() ?? new string[0];
                    }
                });
            }
            catch (CakeException ex)
            {
                CakeLog.Verbose(ex.ToString());
            }

            if (exitCode == 0)
            {
                minVerVersion = ParseVersion(standardOutput);
            }

            version = minVerVersion;
            return exitCode.GetValueOrDefault();
        }

        protected override void ProcessExitCode(int exitCode)
        {
            // Do nothing
        }

        protected abstract ProcessArgumentBuilder GetArguments(MinVerSettings settings);

        /// <summary>
        /// Creates a <see cref="ProcessArgumentBuilder" /> and adds common commandline arguments.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>Instance of <see cref="ProcessArgumentBuilder" />.</returns>
        protected new ProcessArgumentBuilder CreateArgumentBuilder(MinVerSettings settings)
        {
            var args = base.CreateArgumentBuilder(settings);

            AppendAutoIncrement(args, settings);
            AppendBuildMetadata(args, settings);
            AppendDefaultPreReleasePhase(args, settings);
            AppendMinimumMajorMinor(args, settings);
            AppendRepo(args, settings);
            AppendTagPrefix(args, settings);
            AppendVerbosity(args, settings);

            return args;
        }

        private static void AppendAutoIncrement(ProcessArgumentBuilder args, MinVerSettings settings)
        {
            if (!settings.AutoIncrement.HasValue)
            {
                return;
            }

            switch (settings.AutoIncrement.Value)
            {
                case MinVerAutoIncrement.Major:
                    args.Append("--auto-increment major");
                    break;

                case MinVerAutoIncrement.Minor:
                    args.Append("--auto-increment minor");
                    break;

                case MinVerAutoIncrement.Patch:
                    args.Append("--auto-increment patch");
                    break;

                default:
                    throw new CakeException($"{nameof(settings.AutoIncrement)}={(int)settings.AutoIncrement.Value} is invalid");
            }
        }

        private static void AppendBuildMetadata(ProcessArgumentBuilder args, MinVerSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.BuildMetadata))
            {
                return;
            }

            args.Append("--build-metadata");
            args.AppendQuoted(settings.BuildMetadata);
        }

        private static void AppendDefaultPreReleasePhase(ProcessArgumentBuilder args, MinVerSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.DefaultPreReleasePhase))
            {
                return;
            }

            args.Append("--default-pre-release-phase");
            args.AppendQuoted(settings.DefaultPreReleasePhase);
        }

        private static void AppendMinimumMajorMinor(ProcessArgumentBuilder args, MinVerSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.MinimumMajorMinor))
            {
                return;
            }

            args.Append("--minimum-major-minor");
            args.AppendQuoted(settings.MinimumMajorMinor);
        }

        private static void AppendRepo(ProcessArgumentBuilder args, MinVerSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Repo?.FullPath))
            {
                return;
            }

            args.Append("--repo");
            args.AppendQuoted(settings.Repo.FullPath);
        }

        private static void AppendTagPrefix(ProcessArgumentBuilder args, MinVerSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.TagPrefix))
            {
                return;
            }

            args.Append("--tag-prefix");
            args.AppendQuoted(settings.TagPrefix);
        }

        private static void AppendVerbosity(ProcessArgumentBuilder args, MinVerSettings settings)
        {
            var verbosity = settings.Verbosity;
            var toolVerbosity = settings.ToolVerbosity;

            if (!verbosity.HasValue && toolVerbosity.HasValue)
            {
                verbosity = ToolToMinVerVerbosityConverter(toolVerbosity.Value);
            }

            if (!verbosity.HasValue)
            {
                return;
            }

            switch (verbosity)
            {
                case MinVerVerbosity.Error:
                    args.Append("--verbosity error");
                    break;

                case MinVerVerbosity.Warn:
                    args.Append("--verbosity warn");
                    break;

                case MinVerVerbosity.Info:
                    args.Append("--verbosity info");
                    break;

                case MinVerVerbosity.Debug:
                    args.Append("--verbosity debug");
                    break;

                case MinVerVerbosity.Trace:
                    args.Append("--verbosity trace");
                    break;

                default:
                    throw new CakeException($"{nameof(settings.Verbosity)}={(int)verbosity.Value} is invalid");
            }
        }

        private static MinVerVerbosity ToolToMinVerVerbosityConverter(DotNetVerbosity toolVerbosity)
        {
            return toolVerbosity switch
            {
                DotNetVerbosity.Quiet => MinVerVerbosity.Error,
                DotNetVerbosity.Minimal => MinVerVerbosity.Warn,
                DotNetVerbosity.Normal => MinVerVerbosity.Info,
                DotNetVerbosity.Detailed => MinVerVerbosity.Debug,
                DotNetVerbosity.Diagnostic => MinVerVerbosity.Trace,
                _ => MinVerVerbosity.Info
            };
        }

        private static MinVerVersion ParseVersion(IEnumerable<string> standardOutput)
        {
            var version = standardOutput?.LastOrDefault();
            if (string.IsNullOrWhiteSpace(version))
            {
                throw new CakeException($"Version '{version}' is not valid.");
            }

            try
            {
                return new MinVerVersion(version);
            }
            catch (Exception ex)
            {
                throw new CakeException($"Version '{version}' is not valid.", ex);
            }
        }
    }
}
