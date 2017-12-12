using System;

namespace Toffee.Core
{
    public interface ICommandHelper
    {
        int LogAndExit(string reason);
        int LogAndExit<T>(Exception ex);
        int PrintDoneAndExitSuccessfully();
        (bool isValid, int exitCode) ValidateArgs<TCallee, TArgs>(ICommandArgsParser<TArgs> commandArgsParser, string[] args);
    }
}