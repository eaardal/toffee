namespace Toffee
{
    public interface ICommand
    {
        bool CanExecute(string command);
        int Execute(string[] args);
    }
}