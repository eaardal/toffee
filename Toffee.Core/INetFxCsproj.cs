using System.Collections.Generic;

namespace Toffee
{
    public interface INetFxCsproj
    {
        Dictionary<string, string> LinkReferencedNuGetDllsToLocalDlls(string csprojPath, Link link, string[] dlls);
    }
}