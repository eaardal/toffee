using System.Collections.Generic;
using System.IO;
using System.Linq;
using Toffee.Infrastructure;

namespace Toffee
{
    class LinkRegistryFile : ILinkRegistryFile
    {
        private readonly IFilesystem _filesystem;
        private readonly IEnvironment _environment;

        public LinkRegistryFile(IFilesystem filesystem, IEnvironment environment)
        {
            _filesystem = filesystem;
            _environment = environment;
        }

        public void SaveOrUpdateLink(string linkName, string path)
        {
            EnsureLinkRegistryFileExists();

            (var ok, var link) = TryGetLink(linkName);

            if (ok)
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
            var appDataDirectory = _environment.GetAppDataDirectoryPath();
            var toffeeAppDataDirectory = Path.Combine(appDataDirectory, "Toffee");

            if (!_filesystem.DirectoryExists(toffeeAppDataDirectory))
            {
                _filesystem.CreateDirectory(toffeeAppDataDirectory);
            }

            var linkRegistryFile = Path.Combine(toffeeAppDataDirectory, "LinkRegistry.csv");

            if (!_filesystem.FileExists(linkRegistryFile))
            {
                _filesystem.CreateFile(linkRegistryFile);
            }

            return (linkRegistryFile, toffeeAppDataDirectory);
        }

        public (bool ok, Link link) TryGetLink(string linkName)
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

            return lines.Select(Link.ParseFromCsv);
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