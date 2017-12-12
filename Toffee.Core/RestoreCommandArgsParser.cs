using Toffee.Core.Infrastructure;

namespace Toffee.Core
{
    public class RestoreCommandArgsParser : ICommandArgsParser<RestoreCommandArgs>
    {
        private readonly IFilesystem _filesystem;

        public RestoreCommandArgsParser(IFilesystem filesystem)
        {
            _filesystem = filesystem;
        }

        public (bool isValid, string reason) IsValid(string[] args)
        {
            if (args.Length != 2)
            {
                return (false, "Invalid args. Syntax for the \"restore\" command is: \"restore --dest={path}\"");
            }

            var command = args[0];

            if (command != "restore")
            {
                return (false, "First param was not the \"restore\" command");
            }

            var destinationDirectoryPath = args[1];

            if (string.IsNullOrEmpty(destinationDirectoryPath))
            {
                return (false, "Path to {--dest|-d} directory was not set");
            }

            var destinationDirectoryPathParts = destinationDirectoryPath.Split('=');

            if (destinationDirectoryPathParts.Length != 2)
            {
                return (false, "Path to {--dest|-d} directory was not given correctly. It should be --dest={valid-path} or -d={valid-path}. Remember to wrap the path in double quotes if it contains spaces.");
            }

            if (destinationDirectoryPathParts[0] != "--dest" && destinationDirectoryPathParts[0] != "-d")
            {
                return (false, "Path to {--dest|-d} directory was not given correctly. It should be --dest={valid-path} or -d={valid-path}. Could not find the \"--dest|-d\"-part. Remember to wrap the path in double quotes if it contains spaces.");
            }

            if (!_filesystem.DirectoryExists(destinationDirectoryPathParts[1]))
            {
                return (false, "Path to {--dest|-d} directory does not exist. Remember to wrap the path in double quotes if it contains spaces.");
            }
            
            return (true, null);
        }

        public RestoreCommandArgs Parse(string[] args)
        {
            var destinationDirectoryPath = args[1].Split('=')[1];

            return new RestoreCommandArgs(destinationDirectoryPath);
        }
    }
}