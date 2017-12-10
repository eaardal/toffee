using System.Collections.Generic;
using System.IO;
using System.Linq;
using Toffee.Infrastructure;

namespace Toffee
{
    class LinkMapFile : ILinkMapFile
    {
        private readonly IToffeeAppDataDirectory _toffeeAppDataDirectory;
        private readonly IFilesystem _filesystem;

        public LinkMapFile(IToffeeAppDataDirectory toffeeAppDataDirectory, IFilesystem filesystem)
        {
            _toffeeAppDataDirectory = toffeeAppDataDirectory;
            _filesystem = filesystem;
        }

        public void WriteReplacementRecords(string linkName, IReadOnlyCollection<ReplacementRecord> replacementRecords, string destinationDirectoryPath, string csprojPath)
        {
            var linkFilePath = EnsureLinkFileExists(linkName);

            var existingLines = _filesystem.ReadAllLines(linkFilePath).ToArray();

            var lines = replacementRecords
                .Select(record => $"{destinationDirectoryPath},{csprojPath},{record.OriginalReferenceElement},{record.NewReferenceElement},{record.OriginalHintPathElement},{record.NewHintPathElement}")
                .Where(csv => existingLines.All(line => line != csv));

            _filesystem.AppendLines(linkFilePath, lines);
        }

        public IReadOnlyCollection<ReplacementRecord> GetReplacementRecords(string linkName, string destinationDirectoryPath)
        {
            var linkFilePath = EnsureLinkFileExists(linkName);

            var lines = _filesystem.ReadAllLines(linkFilePath);

            return lines.Select(line => line.Split(','))
                .Where(lineParts => lineParts[0] == destinationDirectoryPath)
                .Select(lineParts => new ReplacementRecord
                {
                    OriginalReferenceElement = lineParts[2],
                    NewReferenceElement = lineParts[3],
                    OriginalHintPathElement = lineParts[4],
                    NewHintPathElement = lineParts[5]
                })
                .ToList();
        }

        private string EnsureLinkFileExists(string linkName)
        {
            var toffeeAppDataDirectoryPath = _toffeeAppDataDirectory.EnsureExists();

            var linkFilePath = Path.Combine(toffeeAppDataDirectoryPath, $"link-map__{linkName}.csv");

            if (!_filesystem.FileExists(linkFilePath))
            {
                _filesystem.CreateFile(linkFilePath);
            }

            return linkFilePath;
        }
    }
}