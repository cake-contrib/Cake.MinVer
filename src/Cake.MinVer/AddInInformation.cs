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

using System.Reflection;
using Cake.Core.Diagnostics;

namespace Cake.MinVer
{
    internal static class AddInInformation
    {
        private static readonly Assembly _thisAssembly = typeof(AddInInformation).GetTypeInfo().Assembly;

        private static readonly string _informationalVersion = _thisAssembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

        private static readonly string _assemblyVersion = _thisAssembly
            .GetName().Version.ToString(3);

        private static readonly string _assemblyName = _thisAssembly
            .GetName().Name;

        public static void LogVersionInformation(ICakeLog log)
        {
            log.Verbose(entry =>
                entry("Using add-in: {0} v{1} ({2})", _assemblyName, _assemblyVersion, _informationalVersion));
        }
    }
}
