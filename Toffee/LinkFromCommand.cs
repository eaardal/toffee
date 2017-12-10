using System;
using Toffee.Infrastructure;

namespace Toffee
{
    public class LinkFromCommand : ICommand
    {
        private readonly ICommandArgsParser<LinkFromCommandArgs> _commandArgsParser;
        private readonly ILinkRegistryFile _linkRegistryFile;
        private readonly IUserInterface _ui;

        public LinkFromCommand(ICommandArgsParser<LinkFromCommandArgs> commandArgsParser, ILinkRegistryFile linkRegistryFile, IUserInterface ui)
        {
            _commandArgsParser = commandArgsParser;
            _linkRegistryFile = linkRegistryFile;
            _ui = ui;
        }

        public bool CanExecute(string command)
        {
            return command == "link-from";
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

                _linkRegistryFile.InsertOrUpdateLink(command.LinkName, command.SourceDirectoryPath);

                _ui.WriteLineSuccess($"Created link \"{command.LinkName}\" -> \"{command.SourceDirectoryPath}\"");

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
