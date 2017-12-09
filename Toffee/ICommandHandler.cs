namespace Toffee
{
    public interface ICommandHandler
    {
        int Handle(string command, string[] commandArgs);
    }
}