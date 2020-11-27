using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.MinVer.Utils
{
    internal interface IEnvironmentProvider
    {
        void SetOverrides(IDictionary<string, string> overrides);

        string GetEnvironmentVariable(string name, string defaultValue = null);

        TEnum? GetEnvironmentVariableAsEnum<TEnum>(string name, TEnum? defaultValue = null)
            where TEnum : struct;

        bool? GetEnvironmentVariableAsBool(string name, bool? defaultValue = null);

        FilePath GetEnvironmentVariableAsFilePath(string name, FilePath defaultValue = null);
    }
}
