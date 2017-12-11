using System;
using System.Collections.Generic;
using System.Linq;
using Toffee.Core.Infrastructure;
using Toffee.Core.Infrastructure.DependencyInjection;

namespace Toffee.Core
{
    public class HelpCommand : ICommand
    {
        private readonly IUserInterface _ui;

        public HelpCommand(IUserInterface ui)
        {
            _ui = ui;
        }

        public bool CanExecute(string command)
        {
            return command == "help";
        }

        public int Execute(string[] args)
        {
            var commands = IoC.Resolve<IEnumerable<ICommand>>();

            PrintHeader();
            PrintVersionNumber();
            PrintCommands(commands);

            return ExitCodes.Success;
        }

        private void PrintCommands(IEnumerable<ICommand> commands)
        {
            foreach (var command in commands)
            {
                var helpText = command.GetHelpText();

                PrintCommandAndDescription(helpText);
                PrintArguments(helpText);
                PrintExamples(helpText);
            }
        }

        private void PrintExamples(HelpText helpText)
        {
            _ui.Indent().Write("Examples", ConsoleColor.White).NewLine();

            if (helpText.Examples.Any())
            {
                foreach (var example in helpText.Examples)
                {
                    _ui.Indent().Indent().Write($"- {example}", ConsoleColor.Yellow).NewLine().NewLine();
                }
            }
            else
            {
                _ui.Indent().Indent().Write("None").NewLine();
            }
        }

        private void PrintArguments(HelpText helpText)
        {
            if (helpText.Arguments.Any())
            {
                foreach (var argument in helpText.Arguments)
                {
                    _ui.Indent().Indent().Write($"- {argument.Key}: ", ConsoleColor.DarkCyan)
                        .Write(argument.Value, ConsoleColor.Cyan).NewLine().NewLine();
                }
            }
            else
            {
                _ui.Indent().Indent().Write("N/A").NewLine();
            }
        }

        private void PrintCommandAndDescription(HelpText helpText)
        {
            _ui.WriteLine(helpText.Command, ConsoleColor.Green)
                .WriteLine(helpText.Description)
                .Indent().Write("Arguments", ConsoleColor.White).NewLine();
        }

        private void PrintVersionNumber()
        {
            _ui.Write($"v{AssemblyHelper.GetExecutingAssemblyVersion()}", ConsoleColor.DarkMagenta)
                .NewLine()
                .NewLine();
        }

        private void PrintHeader()
        {
            _ui.Write(Ascii.Toffee, ConsoleColor.Magenta);
        }

        public HelpText GetHelpText()
        {
            return new HelpText()
                .WithCommand("help")
                .WithDescription("Prints information about commands")
                ;
        }
    }
}
