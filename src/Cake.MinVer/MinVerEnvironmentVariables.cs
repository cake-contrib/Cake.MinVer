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

namespace Cake.MinVer
{
    internal sealed class MinVerEnvironmentVariables
    {
        // ReSharper disable InconsistentNaming

        // Environment variables that translate to MinVer arguments
        internal static readonly string MINVERAUTOINCREMENT = $"MINVER{nameof(MinVerSettings.AutoIncrement).ToUpperInvariant()}";
        internal static readonly string MINVERBUILDMETADATA = $"MINVER{nameof(MinVerSettings.BuildMetadata).ToUpperInvariant()}";
        internal static readonly string MINVERDEFAULTPRERELEASEPHASE = $"MINVER{nameof(MinVerSettings.DefaultPreReleasePhase).ToUpperInvariant()}";
        internal static readonly string MINVERMINIMUMMAJORMINOR = $"MINVER{nameof(MinVerSettings.MinimumMajorMinor).ToUpperInvariant()}";
        internal static readonly string MINVERTAGPREFIX = $"MINVER{nameof(MinVerSettings.TagPrefix).ToUpperInvariant()}";
        internal static readonly string MINVERVERBOSITY = $"MINVER{nameof(MinVerSettings.Verbosity).ToUpperInvariant()}";

        // Environment variables that change Cake.MinVer behavior
        internal static readonly string MINVERPREFERGLOBALTOOL = $"MINVER{nameof(MinVerSettings.PreferGlobalTool).ToUpperInvariant()}";
        internal static readonly string MINVERNOFALLBACK = $"MINVER{nameof(MinVerSettings.NoFallback).ToUpperInvariant()}";
        internal static readonly string MINVERTOOLPATH = $"MINVER{nameof(MinVerSettings.ToolPath).ToUpperInvariant()}";

        // ReSharper restore InconsistentNaming
    }
}
