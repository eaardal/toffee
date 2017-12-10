using System;
using System.IO;
using System.Linq;
using Toffee.Infrastructure;

namespace Toffee
{
    public class LinkToCommand : ICommand
    {
        private readonly ICommandArgsParser<LinkToCommandArgs> _commandArgsParser;
        private readonly ILinkRegistryFile _linkRegistryFile;
        private readonly ILinkFile _linkFile;
        private readonly IFilesystem _filesystem;
        private readonly INetFxCsproj _netFxCsProj;
        private readonly IUserInterface _ui;

        public LinkToCommand(
            ICommandArgsParser<LinkToCommandArgs> commandArgsParser, 
            ILinkRegistryFile linkRegistryFile,
            ILinkFile linkFile,
            IFilesystem filesystem, 
            INetFxCsproj netFxCsProj, 
            IUserInterface ui
            )
        {
            _commandArgsParser = commandArgsParser;
            _linkRegistryFile = linkRegistryFile;
            _linkFile = linkFile;
            _filesystem = filesystem;
            _netFxCsProj = netFxCsProj;
            _ui = ui;
        }

        public bool CanExecute(string command)
        {
            return command == "link-to";
        }

        public int Execute(string[] args)
        {
            (var isValid, var reason) = _commandArgsParser.IsValid(args);

            if (!isValid)
            {
                _ui.WriteLineError(reason);

                return ExitCodes.Error;
            }

            try
            {
                var command = _commandArgsParser.Parse(args);

                var link = _linkRegistryFile.GetLink(command.LinkName);

                var csprojs = _filesystem.GetFilesByExtensionRecursively(command.DestinationDirectoryPath, "csproj");

                foreach (var csproj in csprojs)
                {
                    PrintInspectingTextToUi(csproj);

                    if (IsUnrecognizedProjectType(csproj)) continue;

                    ReplaceProjectReferences(csproj, link, command);
                }

                return ExitCodes.Success;
            }
            catch (Exception ex)
            {
                _ui.WriteLineError(ex.Message);

                return ExitCodes.Error;
            }
        }

        private void ReplaceProjectReferences(FileInfo csproj, Link link, LinkToCommandArgs command)
        {
            var replacementRecords =
                _netFxCsProj.ReplaceReferencedNuGetDllsWithLinkDlls(csproj.FullName, link, command.Dlls);

            if (replacementRecords.Any())
            {
                foreach (var record in replacementRecords)
                {
                    PrintReplacementToUi(record);
                }

                _linkFile.WriteReplacedDlls(link.LinkName, replacementRecords, csproj.FullName);
            }
            else
            {
                _ui.Indent()
                    .Write("No changes", ConsoleColor.DarkGray)
                    .End();
            }
        }

        private void PrintReplacementToUi(ReplacementRecord record)
        {
            _ui.Indent()
                .Write("Replaced ", ConsoleColor.DarkGreen)
                .WriteQuoted(record.OriginalReferenceElement, ConsoleColor.Green)
                .NewLine()
                .Indent()
                .Write("With ", ConsoleColor.DarkGreen)
                .WriteQuoted(record.NewReferenceElement, ConsoleColor.Green)
                .NewLine()
                .Indent()
                .Write("Replaced ", ConsoleColor.DarkGreen)
                .WriteQuoted(record.OriginalHintPathElement, ConsoleColor.Green)
                .NewLine()
                .Indent()
                .Write("With ", ConsoleColor.DarkGreen)
                .WriteQuoted(record.NewHintPathElement, ConsoleColor.Green)
                .End();
        }

        private bool IsUnrecognizedProjectType(FileInfo csproj)
        {
            var isDotNetFrameworkProject = _netFxCsProj.IsDotNetFrameworkCsprojFile(csproj.FullName);

            if (!isDotNetFrameworkProject)
            {
                _ui.Indent()
                    .Write("Project type not recognized. For now, only full .NET Framework projects are supported. Skipping...",
                        ConsoleColor.DarkYellow)
                    .End();

                return true;
            }
            return false;
        }

        private void PrintInspectingTextToUi(FileInfo csproj)
        {
            _ui.Write("Inspecting ", ConsoleColor.DarkCyan)
                .WriteQuoted(csproj.Name, ConsoleColor.Cyan)
                .End();
        }
    }
}
