namespace Toffee
{
    public interface ICommandArgsParser<out TArgs>
    {
        (bool isValid, string reason) IsValid(string[] args);
        TArgs Parse(string[] args);
    }
}