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

        public bool CanHandle(string command)
        {
            return command == "link-from";
        }

        public int Handle(string[] args)
        {
            (var isValid, var reason) = _commandArgsParser.IsValid(args);

            if (isValid)
            {
                var command = _commandArgsParser.Parse(args);

                _linkRegistryFile.SaveOrUpdateLink(command.LinkName, command.SourceDirectoryPath);

                return ExitCodes.Success;
            }

            _ui.WriteLineError(reason);

            return ExitCodes.Error;
        }
    }
}
