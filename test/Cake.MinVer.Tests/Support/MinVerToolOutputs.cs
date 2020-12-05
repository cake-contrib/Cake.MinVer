// Copyright 2020 C. Augusto Proiete & Contributors
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

using System.Linq;

namespace Cake.MinVer.Tests.Support
{
    internal static class MinVerToolOutputs
    {
        internal static readonly string[] OutputWhenNotAGitRepo =
        {
            "MinVer: warning : '.' is not a valid working directory. Using default version 0.0.0-alpha.0.",
            "MinVer: Calculated version 0.0.0-alpha.0.",
            "0.0.0-alpha.0",
        };

        internal static readonly string[] OutputWhenTagNotFound =
        {
            "MinVer: No commit found with a valid SemVer 2.0 version. Using default version 0.0.0-alpha.0.",
            "MinVer: Using { Commit: d34db33, Tag: null, Version: 0.0.0-alpha.0, Height: 42 }.",
            "MinVer: Calculated version 0.0.0-alpha.0.42.",
            "0.0.0-alpha.0.42",
        };

        internal static readonly string[] OutputWhenTagFoundDefaultVerbosity =
        {
            "MinVer: Using { Commit: d34db33, Tag: 'v5.0.0', Version: 5.0.0, Height: 8 }.",
            "MinVer: Calculated version 5.0.1-alpha.0.8.",
            "5.0.1-alpha.0.8",
        };

        internal static readonly string[] OutputWhenTagFoundVerbosityError =
        {
            "1.2.3-preview.0.4",
        };

        internal static readonly string[] DefaultOutputForLocalTool = OutputWhenTagFoundDefaultVerbosity;
        internal static readonly string[] DefaultOutputForGlobalTool = OutputWhenTagFoundVerbosityError;

        internal static readonly MinVerVersion DefaultVersionForLocalTool = new MinVerVersion(OutputWhenTagFoundDefaultVerbosity.Last());
        internal static readonly MinVerVersion DefaultVersionForGlobalTool = new MinVerVersion(OutputWhenTagFoundVerbosityError.Last());
    }
}
