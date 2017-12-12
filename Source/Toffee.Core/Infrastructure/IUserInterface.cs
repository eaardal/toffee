using System;

namespace Toffee.Core.Infrastructure
{
    public interface IUserInterface
    {
        IUserInterface WriteLine(string text, ConsoleColor color = ConsoleColor.Gray);
        IUserInterface WriteLineError(string text);
        IUserInterface WriteLineSuccess(string text);
        IUserInterface Write(string text, ConsoleColor color = ConsoleColor.Gray);
        void End();
        IUserInterface NewLine();
        IUserInterface Indent();
        IUserInterface WriteLineWarning(string text);
        IUserInterface WriteQuoted(string text, ConsoleColor color);
    }
}
