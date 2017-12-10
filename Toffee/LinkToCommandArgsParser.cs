using System;
using System.IO;
using System.Linq;
using Toffee.Infrastructure;

namespace Toffee
{
    public class LinkToCommandArgsParser : ICommandArgsParser<LinkToCommandArgs>
    {
        private readonly IFilesystem _filesystem;
        private readonly ILinkRegistryFile _linkRegistryFile;

        public LinkToCommandArgsParser(IFilesystem filesystem, ILinkRegistryFile linkRegistryFile)
        {
            _filesystem = filesystem;
            _linkRegistryFile = linkRegistryFile;
        }

        public (bool isValid, string reason) IsValid(string[] args)
        {
            var command = args[0];

            if (command != "link-to")
            {
                return (false, "First param was not the link-to command");
            }

            var destinationDirectoryPath = args[1];

            if (string.IsNullOrEmpty(destinationDirectoryPath))
            {
                return (false, "Path to destination directory was not set");
            }

            var destinationDirectoryPathParts = destinationDirectoryPath.Split('=');

            if (destinationDirectoryPathParts.Length != 2)
            {
                return (false, "Path to destination director was not given correctly. It should be dest={valid-path}. Remember to wrap the path in double quotes if it contains spaces.");
            }

            if (destinationDirectoryPathParts[0] != "dest")
            {
                return (false, "Path to destination directory was not given correctly. It should be dest={valid-path}. Could not find the \"dest\"-part. Remember to wrap the path in double quotes if it contains spaces.");
            }

            if (!_filesystem.DirectoryExists(destinationDirectoryPathParts[1]))
            {
                return (false, "Path to destination directory does not exist. Remember to wrap the path in double quotes if it contains spaces.");
            }

            var linkNameArg = args[2];

            if (string.IsNullOrEmpty(linkNameArg))
            {
                return (false, "Link name was not set");
            }

            var linkNameParts = linkNameArg.Split('=');

            if (linkNameParts.Length != 2)
            {
                return (false, "Link name was not given correctly. It should be from={link-name}. Link name should not contain spaces and be lower case");
            }

            if (linkNameParts[0] != "from")
            {
                return (false, "Link name was not given correctly. It should be from={link-name}. Could not find the \"from\"-part");
            }

            if (linkNameParts[1].Contains(" "))
            {
                return (false, "Link name can not contain spaces");
            }

            var linkName = linkNameParts[1];

            (var foundLink, var link) = _linkRegistryFile.TryGetLink(linkName);

            if (!foundLink)
            {
                return (false, $"The link \"{linkName}\" does not exist in the registry");
            }

            var dllsArg = args[3];

            if (string.IsNullOrEmpty(dllsArg))
            {
                return (false, "List of dlls to link was not set. It should be dlls={comma-separated-list-of-dll-names-with-no-spaces}");
            }

            var dllsParts = dllsArg.Split('=');

            if (dllsParts.Length != 2)
            {
                return (false, "List of dlls to link was not given correctly. It should be using={comma-separated-list-of-dll-names-with-no-spaces}");
            }

            if (dllsParts[0] != "using")
            {
                return (false, "List of dlls was not given correctly. It should be using={comma-separated-list-of-dll-names-with-no-spaces}. Could not find the \"using\"-part");
            }

            if (dllsParts[1].Contains(" "))
            {
                return (false, "List of dlls can not contain spaces");
            }

            var dlls = dllsParts[1].Split(',');

            foreach (var dll in dlls)
            {
                var fullDllPath = Path.Combine(link.SourceDirectoryPath, dll);

                if (!_filesystem.FileExists($"{fullDllPath}.dll"))
                {
                    return (false, $"The DLL \"{fullDllPath}\" does not exist. The path was constructed by combining the link's ({linkName}) Source Directory ({link.SourceDirectoryPath}) and the entered DLL {dll}. One of these values must be adjusted to make a valid path");
                }
            }

            return (true, null);
        }

        public LinkToCommandArgs Parse(string[] args)
        {
            var destinationDirectoryPath = args[1].Split('=')[1];
            var linkName = args[2].Split('=')[1];
            var dlls = args[3].Split('=')[1].Split(',').Select(d => d.EndsWith(".dll") ? d.Substring(0, d.Length - 4) : d).ToArray();

            return new LinkToCommandArgs(destinationDirectoryPath, linkName, dlls);
        }
    }
}
