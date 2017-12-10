using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Toffee.Infrastructure;

namespace Toffee
{
    class NetFxCsproj : INetFxCsproj
    {
        private const string Indentation6 = "      ";
        private const string Indentation4 = "    ";

        private readonly IFilesystem _filesystem;

        public NetFxCsproj(IFilesystem filesystem)
        {
            _filesystem = filesystem;
        }
        
        public bool IsDotNetFrameworkCsprojFile(string path)
        {
            var lines = _filesystem.ReadAllLines(path).Where(line => !string.IsNullOrEmpty(line)).ToArray();
            var firstLineIsXmlDeclaration = lines.ElementAt(0) == "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            var secondLineIsProjectElementWithToolsVersionAttr = lines.ElementAt(1).StartsWith("<Project ToolsVersion");

            return firstLineIsXmlDeclaration && secondLineIsProjectElementWithToolsVersionAttr;
        }

        public IReadOnlyCollection<ReplacementRecord> ReplaceLinkedDllsWithOriginalNuGetDlls(string csprojPath)
        {
            var replacedLines = new List<ReplacementRecord>();

            var lines = _filesystem.ReadAllLines(csprojPath).ToArray();

            var linesCopy = new List<string>();

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                if (IsEndOfFile(i, lines))
                {
                    linesCopy.Add(line);
                    continue;
                }
                
                var isCommentForReplacedReference = IsCommentForReplacedReference(line);

                if (isCommentForReplacedReference)
                {
                    var replacementRecords = new ReplacementRecord();
                    
                    var originalReference = ExtractOriginalReference(line);
                    linesCopy.Add(originalReference);

                    replacementRecords.OriginalReferenceElement

                    i++;
                }
                else
                {
                    linesCopy.Add(line);
                }
            }

            if (replacedLines.Any())
            {
                _filesystem.WriteAllLines(csprojPath, linesCopy);
            }

            return replacedLines;
        }

        private static string ExtractOriginalReference(string line)
        {
            return line.Split("Original line:")[1].Trim();
        }

        private static bool IsCommentForReplacedReference(string line)
        {
            return line.TrimStart().StartsWith("<!-- Line below was replaced by Toffee");
        }

        public IReadOnlyCollection<ReplacementRecord> ReplaceReferencedNuGetDllsWithLinkDlls(string csprojPath, Link link, string[] dlls)
        {
            var replacedLines = new List<ReplacementRecord>();

            var lines = _filesystem.ReadAllLines(csprojPath).ToArray();

            var linesCopy = new List<string>();
            
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                if (IsEndOfFile(i, lines))
                {
                    linesCopy.Add(line);
                    continue;
                }

                var nextLine = lines[i + 1];

                (var isReference, var dll) = IsReferenceToInstalledNuGetPackage(line, dlls);

                var isNextLineHintPath = IsNextLineHintPathPointingToNuGetDll(nextLine);

                if (isReference && isNextLineHintPath)
                {
                    var replacementRecord = new ReplacementRecord();

                    // Write comment above the <Reference Inclue=""> line
                    var commentForReplacedReferenceLine = $"{Indentation4}<!-- Line below was replaced by Toffee at {DateTime.Now:dd.MM.yyyy HH:mm:ss}. Original line: {line.Trim()} -->";
                    linesCopy.Add(commentForReplacedReferenceLine);

                    // Replace the <Reference Include=""> line with assembly info for the linked dll instead of the original dll
                    var replacedReferenceLine = ConstructReferenceLineWithLinkedDllAssemblyInfo(link, dll);
                    linesCopy.Add(replacedReferenceLine);

                    replacementRecord.OriginalReferenceElement = line.Trim();
                    replacementRecord.NewReferenceElement = replacedReferenceLine.Trim();

                    // Write comment above the next <HintPath> line
                    var commentForReplacedHintPathLine = $"{Indentation6}<!-- Line below was replaced by Toffee at {DateTime.Now:dd.MM.yyyy HH:mm:ss}. Original line: {nextLine.Trim()} -->";
                    linesCopy.Add(commentForReplacedHintPathLine);

                    // Replace the next <HintPath>{path}</HintPath> line with path to local dll instead of nuget dll
                    var replacedHintPathLine = ConstructHintPathLineWithLinkedDll(link, dll);
                    linesCopy.Add(replacedHintPathLine);

                    replacementRecord.OriginalHintPathElement = nextLine.Trim();
                    replacementRecord.NewHintPathElement = replacedHintPathLine.Trim();
                    
                    replacedLines.Add(replacementRecord);

                    i++;
                }
                else
                {
                    linesCopy.Add(line);
                }
            }

            if (replacedLines.Any())
            {
                _filesystem.WriteAllLines(csprojPath, linesCopy);
            }

            return replacedLines;
        }

        private string ConstructReferenceLineWithLinkedDllAssemblyInfo(Link link, string dll)
        {
            var linkDllPath = ConstructPathToLinkDll(link, dll);

            if (_filesystem.FileExists(linkDllPath))
            {
                var linkedAssembly = Assembly.LoadFile(linkDllPath);
                return $"{Indentation4}<Reference Include=\"{linkedAssembly.FullName}\">";
            }

            throw new FileNotFoundException($"The constructed path to linked dll \"{linkDllPath}\" does not exist");
        }

        private static bool IsEndOfFile(int i, string[] lines)
        {
            return i + 1 >= lines.Length;
        }

        private static bool IsNextLineHintPathPointingToNuGetDll(string nextLine)
        {
            return nextLine.TrimStart().StartsWith("<HintPath>..\\packages\\");
        }

        private string ConstructHintPathLineWithLinkedDll(Link link, string dll)
        {
            var linkDllPath = ConstructPathToLinkDll(link, dll);

            if (_filesystem.FileExists(linkDllPath))
            {
                return $"{Indentation6}<HintPath>{linkDllPath}</HintPath>";
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
                var s = string.Format("<Reference Include=\"{0}, Version=", dll);

                if (line.TrimStart().StartsWith(s))
                {
                    return (true, dll);
                }
            }

            return (false, null);
        }
    }
}