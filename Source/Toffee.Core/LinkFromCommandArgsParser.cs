using System.IO;
using Toffee.Core.Infrastructure;

namespace Toffee.Core
{
    public class LinkFromCommandArgsParser : ICommandArgsParser<LinkFromCommandArgs>
    {
        private readonly IFilesystem _filesystem;

        public LinkFromCommandArgsParser(IFilesystem filesystem)
        {
            _filesystem = filesystem;
        }

        public (bool isValid, string reason) IsValid(string[] args)
        {
            if (args.Length != 3)
            {
                return (false, "Invalid args. Syntax for the \"link-from\" command is: \"link-from --src={path} --name={link-name}\"");
            }

            var command = args[0];

            if (command != "link-from")
            {
                return (false, "First param was not the \"link-from\" command");
            }

            var sourceDirectoryPathArg = args[1];

            if (string.IsNullOrEmpty(sourceDirectoryPathArg))
            {
                return (false, "Path to {--src|-s} directory was not set");
            }

            var sourceDirectoryPathParts = sourceDirectoryPathArg.Split('=');

            if (sourceDirectoryPathParts.Length != 2)
            {
                return (false, "Path to {--src|-s} was not given correctly. It should be --src={valid-path} or -s={valid-path}. Remember to wrap the path in double quotes if it contains spaces.");
            }

            if (sourceDirectoryPathParts[0] != "--src" && sourceDirectoryPathParts[0] != "-s")
            {
                return (false, "Path to source directory was not given correctly. It should be --src={valid-path} or -s={valid-path}. Could not find the \"--src|-s\"-part. Remember to wrap the path in double quotes if it contains spaces.");
            }

            var sourceDirectoryPath = sourceDirectoryPathParts[1];

            if (!_filesystem.DirectoryExists(sourceDirectoryPath))
            {
                return (false, "Path to {--src|-s} directory does not exist. Remember to wrap the path in double quotes if it contains spaces.");
            }

            if (!Path.IsPathRooted(sourceDirectoryPath))
            {
                return (false, "The {--src|-s} directory path must be absolute");
            }

            var linkName = args[2];

            if (string.IsNullOrEmpty(linkName))
            {
                return (false, "Link name was not set");
            }

            var linkNameParts = linkName.Split('=');

            if (linkNameParts.Length != 2)
            {
                return (false, "Link name was not given correctly. It should be --name={link-name} or -n={link-name}. Link name should not contain spaces and be lower case");
            }

            if (linkNameParts[0] != "--name" && linkNameParts[0] != "-n")
            {
                return (false, "Link name was not given correctly. It should be --name={link-name} or -n={link-name}. Could not find the \"--name|-n\"-part");
            }
            
            if (linkNameParts[1].Contains(" "))
            {
                return (false, "Link name can not contain spaces");
            }

            return (true, null);
        }

        public LinkFromCommandArgs Parse(string[] args)
        {
            var sourceDirectoryPath = args[1].Split('=')[1];
            var linkName = args[2].Split('=')[1].ToLower();

            return new LinkFromCommandArgs(sourceDirectoryPath, linkName);
        }
    }
}
