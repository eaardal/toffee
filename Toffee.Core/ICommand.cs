namespace Toffee
{
    public interface ICommand
    {
        bool CanHandle(string command);
        int Handle(string[] args);
    }
}