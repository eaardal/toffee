using System;
using System.IO;
using System.Linq;
using Toffee.Core.Infrastructure;

namespace Toffee.Core
{
    public class RestoreCommand : ICommand
    {
        private readonly ICommandArgsParser<RestoreCommandArgs> _restoreCommandArgsParser;
        private readonly INetFxCsproj _netFxCsproj;
        private readonly IUserInterface _ui;
        private readonly IFilesystem _filesystem;
        private readonly ICommandHelper _commandHelper;

        public RestoreCommand(
            ICommandArgsParser<RestoreCommandArgs> restoreCommandArgsParser,
            INetFxCsproj netFxCsproj,
            IUserInterface ui,
            IFilesystem filesystem,
            ICommandHelper commandHelper
            )
        {
            _restoreCommandArgsParser = restoreCommandArgsParser;
            _netFxCsproj = netFxCsproj;
            _ui = ui;
            _filesystem = filesystem;
            _commandHelper = commandHelper;
        }

        public HelpText HelpText =>
            new HelpText()
                .WithCommand("restore")
                .WithDescription("Restores DLL references to their previous paths")
                .WithArgument("--dest|-d",
                    "Path to the project directory you want to restore. Typically the same path as provided to the \"link-to\" command's {--dest|-d} argument")
                .WithExample(@"toffee restore --dest=C:\ProjectB")
                .WithExample(@"toffee restore -d=C:\ProjectB");

        public bool CanExecute(string command)
        {
            return command == "restore";
        }

        public int Execute(string[] args)
        {
            var argsAreValid = _commandHelper.ValidateArgs<RestoreCommand, RestoreCommandArgs>(_restoreCommandArgsParser, args);

            if (!argsAreValid)
            {
                return ExitCodes.Error;
            }
            
            try
            {
                var command = ParseArgs(args);

                RestoreLinkedDllReferencesInProjectFiles(command);

                return _commandHelper.PrintDoneAndExitSuccessfully();
            }
            catch (Exception ex)
            {
                return _commandHelper.LogAndExit<RestoreCommand>(ex);
            }
        }

        private void RestoreLinkedDllReferencesInProjectFiles(RestoreCommandArgs command)
        {
            var csprojs = _filesystem.GetFilesByExtensionRecursively(command.DestinationDirectoryPath, "csproj");

            foreach (var csproj in csprojs)
            {
                PrintInspectingTextToUi(csproj);

                if (IsUnrecognizedProjectType(csproj)) continue;

                RestoreLinkedDllReferencesInProject(csproj);
            }
        }

        private void RestoreLinkedDllReferencesInProject(FileInfo csproj)
        {
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

        private RestoreCommandArgs ParseArgs(string[] args)
        {
            return _restoreCommandArgsParser.Parse(args);
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
