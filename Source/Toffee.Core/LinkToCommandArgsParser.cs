using System.IO;
using System.Linq;
using Toffee.Core.Infrastructure;

namespace Toffee.Core
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
            if (args.Length != 4)
            {
                return (false, "Invalid args. Syntax for the \"link-to\" command is: \"link-to --dest={path} --link={link-name} --dlls={comma-separated-list-of-dlls-with-no-spaces}\"");
            }

            var command = args[0];

            if (command != "link-to")
            {
                return (false, "First param was not the \"link-to\" command");
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

            if (!Path.IsPathRooted(destinationDirectoryPathParts[1]))
            {
                return (false, "The {--dest|-d} directory path must be absolute");
            }

            var linkNameArg = args[2];

            if (string.IsNullOrEmpty(linkNameArg))
            {
                return (false, "Link name was not set");
            }

            var linkNameParts = linkNameArg.Split('=');

            if (linkNameParts.Length != 2)
            {
                return (false, "Link name was not given correctly. It should be --link={link-name} or -l={link-name}. Link name should not contain spaces and be lower case");
            }

            if (linkNameParts[0] != "--link" && linkNameParts[0] != "-l")
            {
                return (false, "Link name was not given correctly. It should be --link={link-name} or -l={link-name}. Could not find the \"--link|-l\"-part");
            }

            if (linkNameParts[1].Contains(" "))
            {
                return (false, "Link name can not contain spaces");
            }

            var linkName = linkNameParts[1];

            (var foundLink, var link) = _linkRegistryFile.TryGetLink(linkName);

            if (!foundLink)
            {
                return (false, $"The link \"{linkName}\" does not exist in the registry. Did you create it using the \"link-from\" command?");
            }

            var dllsArg = args[3];

            if (string.IsNullOrEmpty(dllsArg))
            {
                return (false, "List of dlls to link was not set. It should be --dlls={comma-separated-list-of-dll-names-with-no-spaces} or -D={dll-names}");
            }

            var dllsParts = dllsArg.Split('=');

            if (dllsParts.Length != 2)
            {
                return (false, "List of dlls to link was not given correctly. It should be dlls={comma-separated-list-of-dll-names-with-no-spaces} or -D={dll-names}");
            }

            if (dllsParts[0] != "--dll" && dllsParts[0] != "-D")
            {
                return (false, "List of dlls was not given correctly. It should be dlls={comma-separated-list-of-dll-names-with-no-spaces} or -D={dll-names}. Could not find the \"--dlls|-D\"-part");
            }

            if (dllsParts[1].Contains(" "))
            {
                return (false, "List of dlls can not contain spaces");
            }

            var dlls = dllsParts[1].Split(',');

            foreach (var dll in dlls)
            {
                var fullDllPath = Path.Combine(link.SourceDirectoryPath, dll);

                var normalizedDllPath = fullDllPath.EndsWith(".dll") ? fullDllPath : $"{fullDllPath}.dll";

                if (!_filesystem.FileExists($"{normalizedDllPath}"))
                {
                    return (false, $"The DLL \"{normalizedDllPath}\" does not exist. The path was constructed by combining the link's ({linkName}) Source Directory ({link.SourceDirectoryPath}) and the entered DLL {dll}. One of these values must be adjusted to make a valid path");
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
