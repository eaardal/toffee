using System;
using System.IO;
using System.Linq;
using Serilog;
using Toffee.Infrastructure;

namespace Toffee
{
    public class RestoreCommand : ICommand
    {
        private readonly ICommandArgsParser<RestoreCommandArgs> _restoreCommandArgsParser;
        private readonly INetFxCsproj _netFxCsproj;
        private readonly IUserInterface _ui;
        private readonly IFilesystem _filesystem;
        private readonly ILogger _logger;

        public RestoreCommand(
            ICommandArgsParser<RestoreCommandArgs> restoreCommandArgsParser,
            INetFxCsproj netFxCsproj,
            IUserInterface ui,
            IFilesystem filesystem,
            ILogger logger
            )
        {
            _restoreCommandArgsParser = restoreCommandArgsParser;
            _netFxCsproj = netFxCsproj;
            _ui = ui;
            _filesystem = filesystem;
            _logger = logger;
        }

        public bool CanExecute(string command)
        {
            return command == "restore";
        }

        public int Execute(string[] args)
        {
            try
            {
                (var isValid, var reason) = _restoreCommandArgsParser.IsValid(args);

                if (!isValid)
                {
                    _ui.WriteLineError(reason);
                    PrintDone();

                    return ExitCodes.Error;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while validating the arguments for {nameof(RestoreCommand)}");

                _ui.WriteLineError(ex.Message);
                PrintDone();

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
                    }
                    else
                    {
                        _ui.Indent()
                            .Write("No changes", ConsoleColor.DarkGray)
                            .End();
                    }
                }

                PrintDone();

                return ExitCodes.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while executing {nameof(RestoreCommand)}");

                _ui.WriteLineError(ex.Message);
                PrintDone();

                return ExitCodes.Error;
            }
        }

        private void PrintDone()
        {
            _ui.Write("Done", ConsoleColor.White).End();
        }

        private void PrintReplacementToUi(ReplacementRecord record)
        {
            _ui.Indent()
                .Write("Replaced ", ConsoleColor.DarkGreen)
                .WriteQuoted(record.Before, ConsoleColor.Green)
                .NewLine()
                .Indent()
                .Write("With ", ConsoleColor.DarkGreen)
                .WriteQuoted(record.After, ConsoleColor.Green)
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
