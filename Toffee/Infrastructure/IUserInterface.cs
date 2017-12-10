using System;

namespace Toffee.Infrastructure
{
    public interface IUserInterface
    {
        void WriteLine(string text);
        void WriteLineError(string text);
        void WriteLineSuccess(string text);
        IUserInterface Write(string text, ConsoleColor color);
        void End();
        IUserInterface NewLine();
        IUserInterface Indent();
    }
}
