using System.IO;

namespace Toffee
{
    public class LinkFromCommandArgsParser : ICommandArgsParser<LinkFromCommandArgs>
    {
        public (bool isValid, string reason) IsValid(string[] args)
        {
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

            if (linkName.Contains(" "))
            {
                return (false, "Link name can not contain spaces");
            }

            return (true, null);
        }

        public LinkFromCommandArgs Parse(string[] args)
        {
            var sourceDirectoryPath = args[1].Split()[1];
            var linkName = args[2];

            return new LinkFromCommandArgs(sourceDirectoryPath, linkName);
        }
    }
}
