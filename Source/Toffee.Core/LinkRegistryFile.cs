using System.Collections.Generic;
using System.IO;
using System.Linq;
using Toffee.Core.Infrastructure;

namespace Toffee.Core
{
    class LinkRegistryFile : ILinkRegistryFile
    {
        private readonly IFilesystem _filesystem;
        private readonly IToffeeAppDataDirectory _toffeeAppDataDirectory;

        public LinkRegistryFile(IFilesystem filesystem, IToffeeAppDataDirectory toffeeAppDataDirectory)
        {
            _filesystem = filesystem;
            _toffeeAppDataDirectory = toffeeAppDataDirectory;
        }

        public void InsertOrUpdateLink(string linkName, string path)
        {
            EnsureLinkRegistryFileExists();

            (var exists, var link) = TryGetLink(linkName);

            if (exists)
            {
                UpdateLink(link.LinkName, path);
            }
            else
            {
                InsertLink(linkName, path);
            }
        }

        private void InsertLink(string linkName, string path)
        {
            (var filePath, var _) = EnsureLinkRegistryFileExists();

            var csv = $"{linkName},{path}";

            _filesystem.AppendLine(filePath, csv);
        }

        private void UpdateLink(string linkName, string path)
        {
            (var filePath, var _) = EnsureLinkRegistryFileExists();

            var lines = _filesystem.ReadAllLines(filePath);

            var linesCopy = new List<string>();

            foreach (var line in lines)
            {
                if (line.StartsWith(linkName))
                {
                    var updatedLine = $"{linkName},{path}";
                    linesCopy.Add(updatedLine);
                }
                else
                {
                    linesCopy.Add(line);
                }
            }

            _filesystem.WriteAllLines(filePath, linesCopy);
        }

        private (string filePath, string directoryPath) EnsureLinkRegistryFileExists()
        {
            var toffeeAppDataDirectoryPath = _toffeeAppDataDirectory.EnsureExists();

            var linkRegistryFile = Path.Combine(toffeeAppDataDirectoryPath, "LinkRegistry.csv");

            if (!_filesystem.FileExists(linkRegistryFile))
            {
                _filesystem.CreateFile(linkRegistryFile);
            }

            return (linkRegistryFile, toffeeAppDataDirectoryPath);
        }

        public (bool exists, Link link) TryGetLink(string linkName)
        {
            var link = GetAllLinks().SingleOrDefault(l => l.LinkName == linkName);

            if (link != null)
            {
                return (true, link);
            }

            return (false, null);
        }

        public IEnumerable<Link> GetAllLinks()
        {
            (var filePath, var _) = EnsureLinkRegistryFileExists();

            var lines = _filesystem.ReadAllLines(filePath);

            return lines.Where(line => !string.IsNullOrEmpty(line)).Select(Link.ParseFromCsv);
        }

        public Link GetLink(string linkName)
        {
            var link = GetAllLinks().SingleOrDefault(l => l.LinkName == linkName);

            if (link != null)
            {
                return link;
            }

            throw new LinkNotFoundException($"The link {linkName} was not found in the registry");
        }
    }
}