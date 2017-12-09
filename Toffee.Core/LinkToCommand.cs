using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toffee.Infrastructure;

namespace Toffee
{
    public class LinkToCommand : ICommand
    {
        private readonly ICommandArgsParser<LinkToCommandArgs> _commandArgsParser;
        private readonly ILinkRegistryFile _linkRegistryFile;
        private readonly IFilesystem _filesystem;
        private readonly INetFxCsproj _netFxCsProj;
        private readonly IUserInterface _ui;

        public LinkToCommand(ICommandArgsParser<LinkToCommandArgs> commandArgsParser, ILinkRegistryFile linkRegistryFile, IFilesystem filesystem, INetFxCsproj netFxCsProj, IUserInterface ui)
        {
            _commandArgsParser = commandArgsParser;
            _linkRegistryFile = linkRegistryFile;
            _filesystem = filesystem;
            _netFxCsProj = netFxCsProj;
            _ui = ui;
        }

        public bool CanHandle(string command)
        {
            return command == "link-to";
        }

        public int Handle(string[] args)
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
                    var replacedDlls = _netFxCsProj.LinkReferencedNuGetDllsToLocalDlls(csproj.FullName, link, command.Dlls.ToArray());

                    foreach (var replacedDll in replacedDlls)
                    {
                        _ui.WriteLineSuccess($"{csproj.Name}: Replaced \"{replacedDll.Key}\" -> \"{replacedDll.Value}\"");
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
