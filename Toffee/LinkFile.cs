using System.Collections.Generic;
using System.IO;
using System.Linq;
using Toffee.Infrastructure;

namespace Toffee
{
    class LinkFile : ILinkFile
    {
        private readonly IToffeeAppDataDirectory _toffeeAppDataDirectory;
        private readonly IFilesystem _filesystem;

        public LinkFile(IToffeeAppDataDirectory toffeeAppDataDirectory, IFilesystem filesystem)
        {
            _toffeeAppDataDirectory = toffeeAppDataDirectory;
            _filesystem = filesystem;
        }

        public void WriteReplacedDlls(string linkName, IReadOnlyCollection<ReplacementRecord> replacementRecords, string csprojPath)
        {
            var toffeeAppDataDirectoryPath = _toffeeAppDataDirectory.EnsureExists();

            var linkFilePath = Path.Combine(toffeeAppDataDirectoryPath, $"link-map__{linkName}.csv");

            if (!_filesystem.FileExists(linkFilePath))
            {
                _filesystem.CreateFile(linkFilePath);
            }

            var existingLines = _filesystem.ReadAllLines(linkFilePath).ToArray();

            var lines = replacementRecords
                .Select(record => $"{csprojPath},{record.OriginalReferenceElement},{record.NewReferenceElement},{record.OriginalHintPathElement},{record.NewHintPathElement}")
                .Where(csv => existingLines.All(line => line != csv));

            _filesystem.AppendLines(linkFilePath, lines);
        }
    }
}