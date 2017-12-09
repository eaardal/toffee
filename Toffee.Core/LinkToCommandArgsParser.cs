using System;
using Toffee.Infrastructure;

namespace Toffee
{
    public class LinkToCommandArgsParser : ICommandArgsParser<LinkToCommandArgs>
    {
        private readonly IFilesystem _filesystem;

        public LinkToCommandArgsParser(IFilesystem filesystem)
        {
            _filesystem = filesystem;
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

            var linkName = args[2];

            if (string.IsNullOrEmpty(linkName))
            {
                return (false, "Link name was not set");
            }

            var linkNameParts = linkName.Split('=');

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

            var dlls = args[3];

            if (string.IsNullOrEmpty(dlls))
            {
                return (false, "List of dlls to link was not set. It should be dlls={comma-separated-list-of-dll-names-with-no-spaces}");
            }

            var dllsParts = dlls.Split('=');

            if (dllsParts.Length != 2)
            {
                return (false, "List of dlls to link was not given correctly. It should be dlls={comma-separated-list-of-dll-names-with-no-spaces}");
            }

            if (dllsParts[0] != "from")
            {
                return (false, "List of dlls was not given correctly. It should be dlls={comma-separated-list-of-dll-names-with-no-spaces}. Could not find the \"dlls\"-part");
            }

            if (dllsParts[1].Contains(" "))
            {
                return (false, "List of dlls can not contain spaces");
            }

            // TODO: Validate dlls exists in the link's source directory

            return (true, null);
        }

        public LinkToCommandArgs Parse(string[] args)
        {
            var destinationDirectoryPath = args[1].Split('=')[1];
            var linkName = args[2].Split('=')[1];
            var dlls = args[3].Split('=')[1].Split(',');

            return new LinkToCommandArgs(destinationDirectoryPath, linkName, dlls);
        }
    }
}
