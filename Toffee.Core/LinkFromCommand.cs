using System;
using Serilog;
using Toffee.Core.Infrastructure;

namespace Toffee.Core
{
    public class LinkFromCommand : ICommand
    {
        private readonly ICommandArgsParser<LinkFromCommandArgs> _commandArgsParser;
        private readonly ILinkRegistryFile _linkRegistryFile;
        private readonly IUserInterface _ui;
        private readonly ILogger _logger;

        public LinkFromCommand(ICommandArgsParser<LinkFromCommandArgs> commandArgsParser, ILinkRegistryFile linkRegistryFile, IUserInterface ui, ILogger logger)
        {
            _commandArgsParser = commandArgsParser;
            _linkRegistryFile = linkRegistryFile;
            _ui = ui;
            _logger = logger;
        }

        public bool CanExecute(string command)
        {
            return command == "link-from";
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
                _logger.Error(ex, $"Error occurred while validating the arguments for {nameof(LinkFromCommand)}");

                _ui.WriteLineError(ex.Message);
                PrintDone();

                return ExitCodes.Error;
            }
            
            try
            {
                var command = _commandArgsParser.Parse(args);

                _linkRegistryFile.InsertOrUpdateLink(command.LinkName, command.SourceDirectoryPath);

                _ui.Write("Created link ", ConsoleColor.DarkGreen)
                    .WriteQuoted(command.LinkName, ConsoleColor.Green)
                    .Write(" pointing to ", ConsoleColor.DarkGreen)
                    .WriteQuoted(command.SourceDirectoryPath, ConsoleColor.Green)
                    .End();

                PrintDone();

                return ExitCodes.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while executing {nameof(LinkFromCommand)}");

                _ui.WriteLineError(ex.Message);
                PrintDone();

                return ExitCodes.Error;
            }
        }

        public HelpText GetHelpText()
        {
            return new HelpText()
                .WithCommand("link-from")
                .WithDescription("Creates a named reference to the specified {src} folder")
                .WithArgument("src", "Path to directory containing DLL's you want to use in another project. Typically a bin/Debug directory.")
                .WithArgument("as", "Name of the link pointing to the src directory. Should not contain spaces.")
                .WithExample(@"toffee link-from src=C:\ProjectA\bin\Debug as=my-link")
                ;
        }

        private void PrintDone()
        {
            _ui.Write("Done", ConsoleColor.White).End();
        }
    }
}
