namespace Toffee.Core
{
    public interface ICommandHandler
    {
        int Handle(string command, string[] commandArgs);
    }
}