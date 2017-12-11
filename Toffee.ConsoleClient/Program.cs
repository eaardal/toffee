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
