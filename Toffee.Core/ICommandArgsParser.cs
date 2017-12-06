namespace Toffee.Core
{
    public interface ICommandArgsParser<out TArgs>
    {
        bool IsValid(string[] args);
        TArgs Parse(string[] args);
    }
}