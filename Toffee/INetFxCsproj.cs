using System.Collections.Generic;

namespace Toffee
{
    public interface INetFxCsproj
    {
        Dictionary<string, string> ReplaceReferencedNuGetDllsWithLinkDlls(string csprojPath, Link link, string[] dlls);
    }
}