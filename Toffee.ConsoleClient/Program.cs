using System;
using System.Linq;
using Toffee.Core;
using Toffee.Core.Infrastructure;
using Toffee.Core.Infrastructure.Startup;

namespace Toffee.ConsoleClient
{
    internal static class Program
    {
        private static ICommandHandler _commandHandler;

        public static int Main(string[] args)
        {
            /*  toffee link-from src={path-bin-debug} as={link-name} - Lagrer path til bin/debug mappen i prosjektet man vil linke til
             *    
             *      1. Lagre path til bin/debug i en fil
             *      
             *  toffee link-to={path-packages} from={link-name} using={dlls}
             *  
             *      1. Hente path til bin/debug fra fil, basert på link-name
             *      2. Lage symlink for mappen dll'en ligger under, under packages/ mappen i solution
             *      3. Symlinke til bin/debug
             *  
             *  link-from src=Spv.Logging\bin\Debug as=spv-logging
             *  link-to dest=Betaling.Api.sln from=spv-logging using=Spv.Logging.Installer.dll,Spv.Logging.dll
             *  
             *      1. Finne packages mappe
             *      2. Iterere over mappenavn og finne matcher til DLL'er
             *      3. For hver match, lage symlink fra NuGet DLL til link-from DLL
             *      
             *  Alt 2
             *  
             *      1. Finne alle .csproj filer
             *      2. Erstatte HintPath med path til link-from
             *      
             */

            Startup();

            return BuildConfiguration.IsDebug() ? Repl() : Run(args);
        }

        private static void Startup()
        {
            var ioc = Bootstrapper.Wire();
            _commandHandler = ioc.Resolve<ICommandHandler>();
        }

        private static int Repl()
        {
            var input = string.Empty;
            while (input != "exit")
            {
                Console.WriteLine("Enter command (\"exit\" to exit):");
                input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    var args = input.Split(' ');
                    Run(args);
                }
            }

            return ExitCodes.Success;
        }

        private static int Run(string[] args)
        {
            if (!args.Any())
            {
                // TODO: Show help text instead
                return ExitCodes.Success;
            }

            var command = args.ElementAt(0);

            return _commandHandler.Handle(command, args);
        }
    }
}
