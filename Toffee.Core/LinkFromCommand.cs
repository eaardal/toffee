using System;

namespace Toffee.Core
{
    public class LinkFromCommand : ICommand
    {
        private readonly ICommandArgsParser<LinkFromCommandArgs> _commandArgsParser;

        public LinkFromCommand(ICommandArgsParser<LinkFromCommandArgs> commandArgsParser)
        {
            _commandArgsParser = commandArgsParser;
        }

        public bool CanHandle(string command)
        {
            return command == "link-from";
        }

        public int Handle(string[] args)
        {
            if (!_commandArgsParser.IsValid(args))
            {
                return ExitCodes.Error;
            }

            var command = _commandArgsParser.Parse(args);

            

            return ExitCodes.Success;
        }
    }
}
