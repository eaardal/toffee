using System.Collections.Generic;

namespace Toffee
{
    public interface INetFxCsproj
    {
        IReadOnlyCollection<ReplacementRecord> ReplaceReferencedNuGetDllsWithLinkDlls(string csprojPath, Link link, string[] dlls);
    }
}