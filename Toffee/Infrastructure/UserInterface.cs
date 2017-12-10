using System;

namespace Toffee.Infrastructure
{
    class UserInterface : IUserInterface
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public void WriteLineError(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public void WriteLineSuccess(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public IUserInterface Write(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
            return this;
        }

        public void End()
        {
            Console.Write(Environment.NewLine);
        }

        public IUserInterface NewLine()
        {
            Console.Write(Environment.NewLine);
            return this;
        }

        public IUserInterface Indent()
        {
            Console.Write("  ");
            return this;
        }

        public void WriteLineWarning(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public IUserInterface WriteQuoted(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write($"\"{text}\"");
            Console.ResetColor();
            return this;
        }
    }
}