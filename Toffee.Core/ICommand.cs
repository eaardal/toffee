namespace Toffee.Core
{
    public interface ICommand
    {
        bool CanHandle(string command);
        int Handle(string[] args);
    }
}