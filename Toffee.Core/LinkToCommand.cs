using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Toffee.Core.Infrastructure;

namespace Toffee.Core
{
    public class LinkToCommand : ICommand
    {
        private readonly ICommandArgsParser<LinkToCommandArgs> _commandArgsParser;
        private readonly ILinkRegistryFile _linkRegistryFile;
        private readonly IFilesystem _filesystem;
        private readonly INetFxCsproj _netFxCsproj;
        private readonly IUserInterface _ui;
        private readonly ICommandHelper _commandHelper;

        public LinkToCommand(
            ICommandArgsParser<LinkToCommandArgs> commandArgsParser, 
            ILinkRegistryFile linkRegistryFile,
            IFilesystem filesystem, 
            INetFxCsproj netFxCsproj, 
            IUserInterface ui,
            ICommandHelper commandHelper
            )
        {
            _commandArgsParser = commandArgsParser;
            _linkRegistryFile = linkRegistryFile;
            _filesystem = filesystem;
            _netFxCsproj = netFxCsproj;
            _ui = ui;
            _commandHelper = commandHelper;
        }

        public HelpText HelpText =>
            new HelpText()
                .WithCommand("link-to")
                .WithDescription(
                    "Finds references to the given DLLs in all .csproj's and replaces them with DLLs found in the specified link's {src} directory.")
                .WithArgument("--dest|-d",
                    "Path to the project directory where you want to use the DLL's from a link you've made, instead of the original NuGet reference. Typically the project's git root directory, or the same directory your .sln lives. Csprojs are found recursively below this directory.")
                .WithArgument("--link|-l", "Name of the link to use, as entered when using the link-from command")
                .WithArgument("--dlls|-D",
                    "Comma separated list of DLL's to replace in csprojs, with DLLs found in the named link's {src} directory instead. The .dll extension can be omitted")
                .WithExample(@"toffee link-to --dest=C:\ProjectB --link=my-link --dlls=ProjectA.dll,ProjectC.dll")
                .WithExample(@"toffee link-to -d=C:\ProjectB -l=my-link -D=ProjectA.dll,ProjectC.dll");

        public bool CanExecute(string command)
        {
            return command == "link-to";
        }

        public int Execute(string[] args)
        {
            (var isValid, var exitCode) = _commandHelper.ValidateArgs<LinkToCommand, LinkToCommandArgs>(_commandArgsParser, args);

            if (!isValid)
            {
                return exitCode;
            }
            
            try
            {
                var command = ParseArgs(args);
                var link = GetLink(command);

                ReplaceDllReferencesInProjectFiles(command, link);

                return _commandHelper.PrintDoneAndExitSuccessfully();
            }
            catch (Exception ex)
            {
                return _commandHelper.LogAndExit<LinkToCommand>(ex);
            }
        }

        private void ReplaceDllReferencesInProjectFiles(LinkToCommandArgs command, Link link)
        {
            var csprojs = GetProjectFiles(command);

            foreach (var csproj in csprojs)
            {
                PrintInspectingTextToUi(csproj);

                if (IsUnrecognizedProjectType(csproj)) continue;

                ReplaceDllReferencesInProject(csproj, link, command);
            }
        }

        private IEnumerable<FileInfo> GetProjectFiles(LinkToCommandArgs command)
        {
            return _filesystem.GetFilesByExtensionRecursively(command.DestinationDirectoryPath, "csproj");
        }

        private Link GetLink(LinkToCommandArgs command)
        {
            return _linkRegistryFile.GetLink(command.LinkName);
        }

        private LinkToCommandArgs ParseArgs(string[] args)
        {
            return _commandArgsParser.Parse(args);
        }

        private void ReplaceDllReferencesInProject(FileInfo csproj, Link link, LinkToCommandArgs command)
        {
            var replacementRecords =
                _netFxCsproj.ReplaceReferencedNuGetDllsWithLinkDlls(csproj.FullName, link, command.Dlls);

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
