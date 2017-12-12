using System;

namespace Toffee.Core.Infrastructure
{
    class UserInterface : IUserInterface
    {
        public IUserInterface WriteLine(string text, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
            return this;
        }

        public IUserInterface WriteLineError(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
            return this;
        }

        public IUserInterface WriteLineSuccess(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ResetColor();
            return this;
        }

        public IUserInterface Write(string text, ConsoleColor color = ConsoleColor.Gray)
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

        public IUserInterface WriteLineWarning(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ResetColor();
            return this;
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