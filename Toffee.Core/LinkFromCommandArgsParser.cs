using System.IO;

namespace Toffee
{
    public class LinkFromCommandArgsParser : ICommandArgsParser<LinkFromCommandArgs>
    {
        public (bool isValid, string reason) IsValid(string[] args)
        {
            if (args.Length != 3)
            {
                return (false, "Invalid args. Syntax for the \"link-from\" command is: \"link-from src={path} as={link-name}\"");
            }
            var command = args[0];

            if (command != "link-from")
            {
                return (false, "First param was not the link-from command");
            }

            var sourceDirectoryPath = args[1];

            if (string.IsNullOrEmpty(sourceDirectoryPath))
            {
                return (false, "Path to source directory was not set");
            }

            var sourceDirectoryPathParts = sourceDirectoryPath.Split('=');

            if (sourceDirectoryPathParts.Length != 2)
            {
                return (false, "Path to source directory was not given correctly. It should be src={valid-path}. Remember to wrap the path in double quotes if it contains spaces.");
            }

            if (sourceDirectoryPathParts[0] != "src")
            {
                return (false, "Path to source directory was not given correctly. It should be src={valid-path}. Could not find the \"src\"-part. Remember to wrap the path in double quotes if it contains spaces.");
            }
            
            if (!Directory.Exists(sourceDirectoryPathParts[1]))
            {
                return (false, "Path to source directory does not exist. Remember to wrap the path in double quotes if it contains spaces.");
            }

            var linkName = args[2];

            if (string.IsNullOrEmpty(linkName))
            {
                return (false, "Link name was not set");
            }

            var linkNameParts = linkName.Split('=');

            if (linkNameParts.Length != 2)
            {
                return (false, "Link name was not given correctly. It should be as={link-name}. Link name should not contain spaces and be lower case");
            }

            if (linkNameParts[0] != "as")
            {
                return (false, "Link name was not given correctly. It should be as={link-name}. Could not find the \"as\"-part");
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
