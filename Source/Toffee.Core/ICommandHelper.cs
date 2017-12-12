using System;

namespace Toffee.Core
{
    public interface ICommandHelper
    {
        int LogAndExit(string reason);
        int LogAndExit<T>(Exception ex);
        int PrintDoneAndExitSuccessfully();
        bool ValidateArgs<TCallee, TArgs>(ICommandArgsParser<TArgs> commandArgsParser, string[] args);
    }
}