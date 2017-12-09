using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Toffee.Infrastructure;

namespace Toffee
{
    class NetFxCsproj : INetFxCsproj
    {
        private readonly IFilesystem _filesystem;

        public NetFxCsproj(IFilesystem filesystem)
        {
            _filesystem = filesystem;
        }

        public Dictionary<string, string> LinkReferencedNuGetDllsToLocalDlls(string csprojPath, Link link, string[] dlls)
        {
            var replacedLines = new Dictionary<string, string>();

            var lines = _filesystem.ReadAllLines(csprojPath).ToArray();

            var linesCopy = new List<string>();

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                
                (var isReference, var dll) = IsReferenceToInstalledNuGetPackage(line, dlls);

                if (isReference)
                {
                    linesCopy.Add(line);

                    var replacedLine = ConstructHintPathLineWithLinkedDll(link, dll);
                    linesCopy.Add(replacedLine);

                    var commentLine = $"<!-- Line above was replaced by Toffee at {DateTime.Now:dd.MM.yyyy HH:mm:ss}. Original line: {line} -->";
                    linesCopy.Add(commentLine);

                    replacedLines.Add(line, replacedLine);

                    i = i + 3; // Skipping next line (now replaced by new dll path), and the one after (comment line)
                }
                else
                {
                    linesCopy.Add(line);

                    i++;
                }
            }

            return replacedLines;
        }

        private string ConstructHintPathLineWithLinkedDll(Link link, string dll)
        {
            var linkDllPath = ConstructPathToLinkDll(link, dll);

            if (_filesystem.FileExists(linkDllPath))
            {
                return $"<HintPath>{linkDllPath}</HintPath>";
            }

            throw new FileNotFoundException($"The constructed path to linked dll \"{linkDllPath}\" does not exist");
        }

        private static string ConstructPathToLinkDll(Link link, string dll)
        {
            return Path.Combine(link.SourceDirectoryPath, dll);
        }

        private static (bool isReference, string dll) IsReferenceToInstalledNuGetPackage(string line, IEnumerable<string> dlls)
        {
            foreach (var dll in dlls)
            {
                if (line.StartsWith($"<Reference Include={dll}, Version"))
                {
                    return (true, dll);
                }
            }

            return (false, null);
        }
    }
}