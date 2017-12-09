using System;
using System.Collections.Generic;
using System.Linq;
using Toffee.Infrastructure.Startup;

namespace Toffee.ConsoleClient
{
    internal static class Program
    {
        private static ICommandHandler _commandHandler;

        public static void Main(string[] args)
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
             */

            Startup();

            if (IsDebug())
            {
                Repl();
            }
            else
            {
                Run(args);
            }
        }

        private static void Startup()
        {
            var ioc = Bootstrapper.Wire();
            _commandHandler = ioc.Resolve<ICommandHandler>();
        }

        private static void Repl()
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
        }

        private static void Run(IReadOnlyCollection<string> args)
        {
            if (!args.Any())
            {
                return;
            }

            var command = args.ElementAt(0);

            _commandHandler.Handle(command, args.ToArray());
        }

        private static bool IsDebug()
        {
#if DEBUG
            return true;
#endif
            return false;
        }
    }
}
