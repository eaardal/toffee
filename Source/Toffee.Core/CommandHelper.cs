using System;
using Serilog;
using Toffee.Core.Infrastructure;

namespace Toffee.Core
{
    public class CommandHelper : ICommandHelper
    {
        private readonly IUserInterface _ui;
        private readonly ILogger _logger;

        public CommandHelper(IUserInterface ui, ILogger logger)
        {
            _ui = ui;
            _logger = logger;
        }

        public int LogAndExit(string reason)
        {
            _ui.WriteLineError(reason);
            _ui.Write("Done", ConsoleColor.White).End();

            return ExitCodes.Error;
        }

        public int LogAndExit<T>(Exception ex)
        {
            _logger.Error(ex, $"Error occurred while executing {typeof(T).FullName}");

            _ui.WriteLineError(ex.Message);
            _ui.Write("Done", ConsoleColor.White).End();

            return ExitCodes.Error;
        }

        public int PrintDoneAndExitSuccessfully()
        {
            _ui.Write("Done", ConsoleColor.White).End();
            return ExitCodes.Success;
        }

        public bool ValidateArgs<TCallee, TArgs>(ICommandArgsParser<TArgs> commandArgsParser, string[] args)
        {
            try
            {
                (var isValid, var reason) = commandArgsParser.IsValid(args);

                if (!isValid)
                {
                    LogAndExit(reason);
                    return false;
                }
            }
            catch (UserRequestedExecutionStop)
            {
                return false;
            }
            catch (Exception ex)
            {
                LogAndExit<TCallee>(ex);
                return false;
            }

            return true;
        }
    }
}
