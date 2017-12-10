﻿using System;
using System.IO;
using System.Linq;
using Serilog;
using Toffee.Infrastructure;

namespace Toffee
{
    public class LinkToCommand : ICommand
    {
        private readonly ICommandArgsParser<LinkToCommandArgs> _commandArgsParser;
        private readonly ILinkRegistryFile _linkRegistryFile;
        private readonly IFilesystem _filesystem;
        private readonly INetFxCsproj _netFxCsproj;
        private readonly IUserInterface _ui;
        private readonly ILogger _logger;

        public LinkToCommand(
            ICommandArgsParser<LinkToCommandArgs> commandArgsParser, 
            ILinkRegistryFile linkRegistryFile,
            IFilesystem filesystem, 
            INetFxCsproj netFxCsproj, 
            IUserInterface ui,
            ILogger logger
            )
        {
            _commandArgsParser = commandArgsParser;
            _linkRegistryFile = linkRegistryFile;
            _filesystem = filesystem;
            _netFxCsproj = netFxCsproj;
            _ui = ui;
            _logger = logger;
        }

        public bool CanExecute(string command)
        {
            return command == "link-to";
        }

        public int Execute(string[] args)
        {
            try
            {
                (var isValid, var reason) = _commandArgsParser.IsValid(args);

                if (!isValid)
                {
                    _ui.WriteLineError(reason);
                    PrintDone();

                    return ExitCodes.Error;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while validating the arguments for {nameof(LinkToCommand)}");

                _ui.WriteLineError(ex.Message);
                PrintDone();

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

                PrintDone();

                return ExitCodes.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while executing {nameof(LinkToCommand)}");

                _ui.WriteLineError(ex.Message);
                PrintDone();

                return ExitCodes.Error;
            }
        }

        private void ReplaceProjectReferences(FileInfo csproj, Link link, LinkToCommandArgs command)
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

        private void PrintDone()
        {
            _ui.Write("Done", ConsoleColor.White).End();
        }
    }
}
