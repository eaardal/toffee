namespace Toffee
{
    public interface ICommandHandler
    {
        void Handle(string command, string[] commandArgs);
    }
}