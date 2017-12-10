using System;
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
                    _ui.Write("Inspecting ", ConsoleColor.DarkCyan)
                       .Write($"{csproj.FullName}{Environment.NewLine}", ConsoleColor.Cyan);

                    var replacementRecords = _netFxCsProj.ReplaceReferencedNuGetDllsWithLinkDlls(csproj.FullName, link, command.Dlls.ToArray());

                    if (replacementRecords.Any())
                    {
                        foreach (var record in replacementRecords)
                        {
                            _ui.Indent()
                                .Write("Replaced ", ConsoleColor.DarkGreen)
                                .Write($"\"{record.OriginalReferenceElement}\"", ConsoleColor.Green)
                                .NewLine()
                                .Indent()
                                .Write("With ", ConsoleColor.DarkGreen)
                                .Write($"\"{record.NewReferenceElement}\"", ConsoleColor.Green)
                                .NewLine()
                                .Indent()
                                .Write("Replaced ", ConsoleColor.DarkGreen)
                                .Write($"\"{record.OriginalHintPathElement}\"", ConsoleColor.Green)
                                .NewLine()
                                .Indent()
                                .Write("With ", ConsoleColor.DarkGreen)
                                .Write($"\"{record.NewHintPathElement}\"", ConsoleColor.Green)
                                .End();
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

                return ExitCodes.Success;
            }
            catch (Exception ex)
            {
                _ui.WriteLineError(ex.Message);

                return ExitCodes.Error;
            }
        }
    }
}
