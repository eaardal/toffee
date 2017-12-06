namespace Toffee.Core
{
    public interface ICommandHandler
    {
        void Handle(string command, string[] commandArgs);
    }
}