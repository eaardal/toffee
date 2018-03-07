using System;
using System.Collections.Generic;

namespace Toffee.Core.Infrastructure
{
    public interface IUserInterface
    {
        IUserInterface WriteLine(string text, ConsoleColor color = ConsoleColor.Gray);
        void WriteLineError(string text);
        void WriteLineSuccess(string text);
        IUserInterface Write(string text, ConsoleColor color = ConsoleColor.Gray);
        void End();
        IUserInterface NewLine();
        IUserInterface Indent();
        void WriteLineWarning(string text);
        IUserInterface WriteQuoted(string text, ConsoleColor color = ConsoleColor.Gray);
        IUserInterface WriteLines(IEnumerable<string> lines, ConsoleColor color = ConsoleColor.Gray);
        string WriteQuestion(string question, string defaultAnswer = null, bool allowEmpty = false);
        bool Confirmation(string prompt, string data, bool suggestConfirm = true);
        bool Confirmation(string prompt, bool suggestConfirm = true);

        (int selectedIndex, ConsoleKeyInfo keyInfo) AskUserToSelectItem(IReadOnlyCollection<string> items,
            string question);

        (IReadOnlyCollection<int> selectedIndices, ConsoleKeyInfo keyInfo) AskUserToSelectItems(
            IReadOnlyCollection<string> items, string question, bool selectedByDefault = false);
    }
}
