using System;
using System.Collections.Generic;
using System.Linq;

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

        public void WriteLineWarning(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public IUserInterface WriteQuoted(string text, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.Write($"\"{text}\"");
            Console.ResetColor();
            return this;
        }

        public IUserInterface WriteLines(IEnumerable<string> lines, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
            Console.ResetColor();
            return this;
        }

        public string WriteQuestion(string question, string defaultAnswer = null, bool allowEmpty = false)
        {
            Console.ForegroundColor = ConsoleColor.White;

            if (!string.IsNullOrWhiteSpace(defaultAnswer))
            {
                Console.Write($"{question}");

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($" ({defaultAnswer}): ");
            }
            else
            {
                Console.Write($"{question}: ");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            var answer = Console.ReadLine();

            if (!allowEmpty && string.IsNullOrWhiteSpace(answer) && string.IsNullOrEmpty(defaultAnswer))
            {
                return WriteQuestion(question);
            }

            if (string.IsNullOrWhiteSpace(answer) && !string.IsNullOrEmpty(defaultAnswer))
            {
                return defaultAnswer;
            }

            if (allowEmpty && string.IsNullOrWhiteSpace(answer))
            {
                return null;
            }

            Console.ResetColor();
            return answer;
        }

        public bool Confirmation(string prompt, string data, bool suggestConfirm = true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(prompt);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"\n{data}\n");
            Console.ForegroundColor = ConsoleColor.White;

            Console.Write(suggestConfirm ? "Y/n? " : "y/N? ");

            Console.ForegroundColor = ConsoleColor.Green;
            var answer = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(answer))
            {
                return suggestConfirm;
            }

            if (bool.TryParse(answer, out var result))
            {
                return result;
            }

            return answer == "y";
        }

        public bool Confirmation(string prompt, bool suggestConfirm = true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{prompt} ");
            Console.Write(suggestConfirm ? "Y/n? " : "y/N? ");

            Console.ForegroundColor = ConsoleColor.Green;
            var answer = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(answer))
            {
                return suggestConfirm;
            }

            if (bool.TryParse(answer, out var result))
            {
                return result;
            }

            return answer == "y";
        }

        public (IReadOnlyCollection<int> selectedIndices, ConsoleKeyInfo keyInfo) AskUserToSelectItems(IReadOnlyCollection<string> items, string question, bool selectedByDefault = false)
        {
            var selectedIndex = 0;
            var selectedIndices = new List<int>();
            var isFirstRun = true;

            if (selectedByDefault)
            {
                for (var i = 0; i < items.Count; i++)
                {
                    selectedIndices.Add(i);
                }
            }

            Console.CursorVisible = false;

            Console.WriteLine($"{question} (use SPACE to select, and ENTER to proceed)");

            ConsoleKeyInfo keyInfo;

            do
            {
                if (isFirstRun)
                {
                    isFirstRun = false;
                }
                else
                {
                    Console.SetCursorPosition(0, Console.CursorTop - items.Count);
                }

                for (var i = 0; i < items.Count; i++)
                {
                    string text;

                    if (selectedIndex == i)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        text = $"[-] {items.ElementAt(i)}";
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        text = $"[-] {items.ElementAt(i)}";
                    }

                    if (selectedIndices.Contains(i) && selectedIndex == i)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        text = $"[+] {items.ElementAt(i)}";
                    }
                    else if (selectedIndices.Contains(i))
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        text = $"[+] {items.ElementAt(i)}";
                    }

                    Console.WriteLine(text);
                    Console.ResetColor();
                }

                keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (selectedIndex + 1 < items.Count)
                    {
                        selectedIndex++;
                    }
                }

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (selectedIndex - 1 >= 0)
                    {
                        selectedIndex--;
                    }
                }

                if (keyInfo.Key == ConsoleKey.Spacebar)
                {
                    if (selectedIndices.Contains(selectedIndex))
                    {
                        selectedIndices.Remove(selectedIndex);
                    }
                    else
                    {
                        selectedIndices.Add(selectedIndex);
                    }
                }

            } while (keyInfo.Key != ConsoleKey.Escape && keyInfo.Key != ConsoleKey.Enter);

            return (selectedIndices, keyInfo);
        }

        public (int selectedIndex, ConsoleKeyInfo keyInfo) AskUserToSelectItem(IReadOnlyCollection<string> items, string question)
        {
            var selectedIndex = 0;

            Console.CursorVisible = false;

            ConsoleKeyInfo keyInfo;

            do
            {
                Console.Clear();

                Console.WriteLine(question);

                for (var i = 0; i < items.Count; i++)
                {
                    if (selectedIndex == i)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{i + 1}. {items.ElementAt(i)}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine($"{i + 1}. {items.ElementAt(i)}");
                        Console.ResetColor();
                    }
                }

                keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (selectedIndex + 1 < items.Count)
                    {
                        selectedIndex++;
                    }
                }

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (selectedIndex - 1 >= 0)
                    {
                        selectedIndex--;
                    }
                }
            } while (keyInfo.Key != ConsoleKey.Escape && keyInfo.Key != ConsoleKey.Enter);

            return (selectedIndex, keyInfo);
        }
    }
}