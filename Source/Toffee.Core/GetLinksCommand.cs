using System;
using Toffee.Core.Infrastructure;

namespace Toffee.Core
{
    public class GetLinksCommand : ICommand
    {
        private readonly ILinkRegistryFile _linkRegistryFile;
        private readonly IUserInterface _ui;
        private readonly ICommandHelper _commandHelper;

        public GetLinksCommand(ILinkRegistryFile linkRegistryFile, IUserInterface ui, ICommandHelper commandHelper)
        {
            _linkRegistryFile = linkRegistryFile;
            _ui = ui;
            _commandHelper = commandHelper;
        }
        
        public bool CanExecute(string command)
        {
            return command == "get-links";
        }

        public int Execute(string[] args)
        {
            try
            {
                var links = _linkRegistryFile.GetAllLinks();
                foreach (var link in links)
                {
                    _ui.WriteLine($"{link.LinkName}: {link.SourceDirectoryPath}");
                }

                return _commandHelper.PrintDoneAndExitSuccessfully();
            }
            catch (Exception e)
            {
                return _commandHelper.LogAndExit<GetLinksCommand>(e);
            }
        }

        public HelpText HelpText => new HelpText()
            .WithCommand("get-links")
            .WithDescription("Lists all existing links")
            .WithExample("toffee get-links");
    }
}