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
    }
}