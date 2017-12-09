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

        public Dictionary<string, string> ReplaceReferencedNuGetDllsWithLinkDlls(string csprojPath, Link link, string[] dlls)
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

                    var commentLine = $"<!-- Line below was replaced by Toffee at {DateTime.Now:dd.MM.yyyy HH:mm:ss}. Original line: {line} -->";
                    linesCopy.Add(commentLine);

                    var replacedLine = ConstructHintPathLineWithLinkedDll(link, dll);
                    linesCopy.Add(replacedLine);

                    replacedLines.Add(line, replacedLine);

                    i = i + 3; // Skipping next line (comment line), and the one after (HintPath line, now replaced by new dll path)
                }
                else
                {
                    linesCopy.Add(line);

                    i++;
                }
            }

            _filesystem.WriteAllLines(csprojPath, linesCopy);

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
            return Path.Combine(link.SourceDirectoryPath, $"{dll}.dll");
        }

        private static (bool isReference, string dll) IsReferenceToInstalledNuGetPackage(string line, IEnumerable<string> dlls)
        {
            foreach (var dll in dlls)
            {
                if (line.StartsWith($"<Reference Include=\"{dll}, Version"))
                {
                    return (true, dll);
                }
            }

            return (false, null);
        }
    }
}