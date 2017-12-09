﻿using System;

namespace Toffee
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
    }
}