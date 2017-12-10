using System.Collections.Generic;

namespace Toffee
{
    public interface ILinkMapFile
    {
        void WriteReplacementRecords(string linkName, IReadOnlyCollection<ReplacementRecord> replacementRecords,
            string destinationDirectoryPath, string csprojPath);
        IReadOnlyCollection<ReplacementRecord> GetReplacementRecords(string linkName, string destinationDirectoryPath);
    }
}
