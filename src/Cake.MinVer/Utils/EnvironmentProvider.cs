#region Copyright 2020-2022 C. Augusto Proiete & Contributors
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
using Cake.Core;
using Cake.Core.IO;

namespace Cake.MinVer.Utils
{
    internal class EnvironmentProvider : IEnvironmentProvider
    {
        private readonly ICakeEnvironment _cakeEnvironment;
        private IDictionary<string, string> _overrides;

        public EnvironmentProvider(ICakeEnvironment cakeEnvironment)
        {
            _cakeEnvironment = cakeEnvironment ?? throw new ArgumentNullException(nameof(cakeEnvironment));
        }

        public void SetOverrides(IDictionary<string, string> overrides)
        {
            _overrides = overrides ?? throw new ArgumentNullException(nameof(overrides));
        }

        public string GetEnvironmentVariable(string name, string defaultValue = null)
        {
            if (!(_overrides is null) && _overrides.TryGetValue(name, out var value))
            {
                return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
            }

            value = _cakeEnvironment.GetEnvironmentVariable(name);
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return defaultValue;
        }

        public TEnum? GetEnvironmentVariableAsEnum<TEnum>(string name, TEnum? defaultValue = null)
            where TEnum : struct
        {
            var stringValue = GetEnvironmentVariable(name);
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return defaultValue;
            }

            if (!Enum.TryParse(stringValue, ignoreCase: true, out TEnum enumValue))
            {
                return defaultValue;
            }

            if (!Enum.IsDefined(typeof(TEnum), enumValue))
            {
                return defaultValue;
            }

            return enumValue;
        }

        public bool? GetEnvironmentVariableAsBool(string name, bool? defaultValue = null)
        {
            var stringValue = GetEnvironmentVariable(name);
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return defaultValue;
            }

            switch (stringValue.ToLowerInvariant())
            {
                case "1":
                case "true":
                case "yes":
                    return true;
                case "0":
                case "false":
                case "no":
                    return false;
                default:
                    return defaultValue;
            }
        }

        public FilePath GetEnvironmentVariableAsFilePath(string name, FilePath defaultValue = null)
        {
            var stringValue = GetEnvironmentVariable(name);
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return defaultValue;
            }

            return FilePath.FromString(stringValue);
        }
    }
}
