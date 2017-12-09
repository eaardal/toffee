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

        public void WriteReplacedDlls(string linkName, Dictionary<string, string> replacedDlls, string csprojPath)
        {
            var toffeeAppDataDirectoryPath = _toffeeAppDataDirectory.EnsureExists();

            var linkFilePath = Path.Combine(toffeeAppDataDirectoryPath, $"link-map__{linkName}__{csprojPath}.csv");

            if (!_filesystem.FileExists(linkFilePath))
            {
                _filesystem.CreateFile(linkFilePath);
            }

            var lines = replacedDlls.Select(d => $"{d.Key},{d.Value}");

            _filesystem.WriteAllLines(linkFilePath, lines);
        }
    }
}