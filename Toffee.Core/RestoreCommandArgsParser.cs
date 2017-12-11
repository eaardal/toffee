using System.Collections.Generic;
using Toffee.Core.Infrastructure;

namespace Toffee.Core
{
    public class RestoreCommandArgsParser : ICommandArgsParser<RestoreCommandArgs>
    {
        private readonly IFilesystem _filesystem;
        private readonly ILinkRegistryFile _linkRegistryFile;

        public RestoreCommandArgsParser(IFilesystem filesystem, ILinkRegistryFile linkRegistryFile)
        {
            _filesystem = filesystem;
            _linkRegistryFile = linkRegistryFile;
        }

        public (bool isValid, string reason) IsValid(string[] args)
        {
            if (args.Length != 2)
            {
                return (false, "Invalid args. Syntax for the \"restore\" command is: \"restore dest={path}\"");
            }

            var command = args[0];

            if (command != "restore")
            {
                return (false, "First param was not the restore command");
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

            //var linkNameArg = args[2];

            //if (string.IsNullOrEmpty(linkNameArg))
            //{
            //    return (false, "Link name was not set");
            //}

            //if (linkNameArg != "all")
            //{
            //    var linkNameParts = linkNameArg.Split('=');

            //    if (linkNameParts.Length != 2)
            //    {
            //        return (false, "Link name was not given correctly. It should be link={link-name}. Link name should not contain spaces and be lower case");
            //    }

            //    if (linkNameParts[0] != "link")
            //    {
            //        return (false, "Link name was not given correctly. It should be link={link-name}. Could not find the \"link\"-part");
            //    }

            //    if (linkNameParts[1].Contains(" "))
            //    {
            //        return (false, "Link name can not contain spaces");
            //    }

            //    var linkName = linkNameParts[1];

            //    (var foundLink, var _) = _linkRegistryFile.TryGetLink(linkName);

            //    if (!foundLink)
            //    {
            //        return (false, $"The link \"{linkName}\" does not exist in the registry");
            //    }
            //}

            return (true, null);
        }

        public RestoreCommandArgs Parse(string[] args)
        {
            var destinationDirectoryPath = args[1].Split('=')[1];
            //(var links, var allLinks) = ParseLinksToRestore(args);

            return new RestoreCommandArgs(destinationDirectoryPath, new List<string>(), true);
        }

        private (IReadOnlyCollection<string> links, bool allLinks) ParseLinksToRestore(string[] args)
        {
            var linkNameArg = args[2];

            if (linkNameArg == "all")
            {
                return FindAllLinksUsedInSolution(args);
            }

            return ParseLinksListedInArgs(args);
        }

        private (IReadOnlyCollection<string> links, bool allLinks) ParseLinksListedInArgs(string[] args)
        {
            var linkNames = args[2].Split('=')[1].Split(',');
            
            return (linkNames, false);
        }

        private (IReadOnlyCollection<string> links, bool allLinks) FindAllLinksUsedInSolution(string[] args)
        {
            return (new List<string>(), true);
        }
    }
}