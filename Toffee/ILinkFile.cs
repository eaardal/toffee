using System.Collections.Generic;

namespace Toffee
{
    public interface ILinkFile
    {
        void WriteReplacedDlls(string linkName, IReadOnlyCollection<ReplacementRecord> replacementRecords, string csprojPath);
    }
}
