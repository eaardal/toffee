using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Toffee.Infrastructure;

namespace Toffee
{
    public class RestoreCommand : ICommand
    {
        private readonly ICommandArgsParser<RestoreCommandArgs> _restoreCommandArgsParser;
        private readonly INetFxCsproj _netFxCsproj;
        private readonly IUserInterface _ui;
        private readonly IFilesystem _filesystem;

        public RestoreCommand(ICommandArgsParser<RestoreCommandArgs> restoreCommandArgsParser, INetFxCsproj netFxCsproj, IUserInterface ui, IFilesystem filesystem)
        {
            _restoreCommandArgsParser = restoreCommandArgsParser;
            _netFxCsproj = netFxCsproj;
            _ui = ui;
            _filesystem = filesystem;
        }

        public bool CanExecute(string command)
        {
            return command == "restore";
        }

        public int Execute(string[] args)
        {
            /*
             *  toffee restore dest={path} all
             *  toffee restore dest={path} link={link-name}
             *  
             */

            (var isValid, var reason) = _restoreCommandArgsParser.IsValid(args);

            if (!isValid)
            {
                _ui.WriteLineError(reason);

                return ExitCodes.Error;
            }

            try
            {
                var command = _restoreCommandArgsParser.Parse(args);

                var csprojs = _filesystem.GetFilesByExtensionRecursively(command.DestinationDirectoryPath, "csproj");

                foreach (var csproj in csprojs)
                {
                    PrintInspectingTextToUi(csproj);

                    if (IsUnrecognizedProjectType(csproj)) continue;

                    var replacementRecords =
                        _netFxCsproj.ReplaceLinkedDllsWithOriginalNuGetDlls(csproj.FullName);

                    if (replacementRecords.Any())
                    {
                        foreach (var record in replacementRecords)
                        {
                            PrintReplacementToUi(record);
                        }

                        //_linkFile.WriteReplacementRecords(link.LinkName, replacementRecords, csproj.FullName);
                    }
                    else
                    {
                        _ui.Indent()
                            .Write("No changes", ConsoleColor.DarkGray)
                            .End();
                    }
                }
            }
            catch (Exception ex)
            {
                _ui.WriteLineError(ex.Message);

                return ExitCodes.Error;
            }

            return ExitCodes.Success;
        }

        private void ReplaceProjectReferences(FileInfo csproj, Link link, LinkToCommandArgs command)
        {
            
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
            var isDotNetFrameworkProject = _netFxCsproj.IsDotNetFrameworkCsprojFile(csproj.FullName);

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
