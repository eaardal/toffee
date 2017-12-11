namespace Toffee.Core
{
    public interface ICommand
    {
        bool CanExecute(string command);
        int Execute(string[] args);
    }
}