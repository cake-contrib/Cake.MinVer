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
