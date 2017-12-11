using System.Diagnostics;
using System.Reflection;

namespace Toffee.Core.Infrastructure
{
    public class AssemblyHelper
    {
        public static string GetAssemblyVersion(Assembly assembly)
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return versionInfo.FileVersion;
        }

        public static string GetExecutingAssemblyVersion()
        {
            return GetAssemblyVersion(Assembly.GetExecutingAssembly());
        }
    }
}
