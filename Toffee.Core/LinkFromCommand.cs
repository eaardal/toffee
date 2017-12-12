using System;
using Toffee.Core.Infrastructure;

namespace Toffee.Core
{
    public class LinkFromCommand : ICommand
    {
        private readonly ICommandArgsParser<LinkFromCommandArgs> _commandArgsParser;
        private readonly ILinkRegistryFile _linkRegistryFile;
        private readonly IUserInterface _ui;
        private readonly ICommandHelper _commandHelper;

        public LinkFromCommand(ICommandArgsParser<LinkFromCommandArgs> commandArgsParser, ILinkRegistryFile linkRegistryFile, IUserInterface ui, ICommandHelper commandHelper)
        {
            _commandArgsParser = commandArgsParser;
            _linkRegistryFile = linkRegistryFile;
            _ui = ui;
            _commandHelper = commandHelper;
        }

        public HelpText HelpText =>
            new HelpText()
                .WithCommand("link-from")
                .WithDescription("Creates a named reference to the specified {src} folder")
                .WithArgument("src",
                    "Path to directory containing DLL's you want to use in another project. Typically a bin/Debug directory.")
                .WithArgument("as", "Name of the link pointing to the src directory. Should not contain spaces.")
                .WithExample(@"toffee link-from src=C:\ProjectA\bin\Debug as=my-link");

        public bool CanExecute(string command)
        {
            return command == "link-from";
        }

        public int Execute(string[] args)
        {
            (var isValid, var exitCode) = _commandHelper.ValidateArgs<LinkFromCommand, LinkFromCommandArgs>(_commandArgsParser, args);

            if (!isValid)
            {
                return exitCode;
            }
            
            try
            {
                var command = ParseArgs(args);

                CreateLink(command);
                PrintCreatedLinkToUi(command);

                return _commandHelper.PrintDoneAndExitSuccessfully();
            }
            catch (Exception ex)
            {
                return _commandHelper.LogAndExit<LinkFromCommand>(ex);
            }
        }

        private void PrintCreatedLinkToUi(LinkFromCommandArgs command)
        {
            _ui.Write("Created link ", ConsoleColor.DarkGreen)
                .WriteQuoted(command.LinkName, ConsoleColor.Green)
                .Write(" pointing to ", ConsoleColor.DarkGreen)
                .WriteQuoted(command.SourceDirectoryPath, ConsoleColor.Green)
                .End();
        }

        private void CreateLink(LinkFromCommandArgs command)
        {
            _linkRegistryFile.InsertOrUpdateLink(command.LinkName, command.SourceDirectoryPath);
        }

        private LinkFromCommandArgs ParseArgs(string[] args)
        {
            return _commandArgsParser.Parse(args);
        }
    }
}
