using System.Collections.Generic;

namespace Toffee.Core
{
    public interface INetFxCsproj
    {
        IReadOnlyCollection<ReplacementRecord> ReplaceReferencedNuGetDllsWithLinkDlls(string csprojPath, Link link, string[] dlls);
        bool IsDotNetFrameworkCsprojFile(string path);
        IReadOnlyCollection<ReplacementRecord> ReplaceLinkedDllsWithOriginalNuGetDlls(string csprojPath);
    }
}