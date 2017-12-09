﻿using System;
using System.Collections.Generic;
using System.Linq;
using Toffee.Infrastructure;

namespace Toffee
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IEnumerable<ICommand> _commands;
        private readonly IUserInterface _ui;

        public CommandHandler(IEnumerable<ICommand> commands, IUserInterface ui)
        {
            _commands = commands;
            _ui = ui;
        }

        public int Handle(string command, string[] commandArgs)
        {
            var commandHandler = _commands.SingleOrDefault(c => c.CanHandle(command));

            if (commandHandler == null)
            {
                _ui.WriteLineError($"The command \"{command}\" does not match any known commands");
                return ExitCodes.Error;
            }

            return commandHandler.Handle(commandArgs);
        }
    }
}