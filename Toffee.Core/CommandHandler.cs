using System;
using System.Collections.Generic;
using System.Linq;

namespace Toffee
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IEnumerable<ICommand> _commands;

        public CommandHandler(IEnumerable<ICommand> commands)
        {
            if (commands == null) throw new ArgumentNullException(nameof(commands));
            _commands = commands;
        }

        public void Handle(string command, string[] commandArgs)
        {
            foreach (var commandHandler in _commands.Where(c => c.CanHandle(command)))
            {
                commandHandler.Handle(commandArgs);
            }
        }
    }
}