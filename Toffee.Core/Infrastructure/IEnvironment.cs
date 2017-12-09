namespace Toffee.Infrastructure
{
    public interface IEnvironment
    {
        string GetProgramDataDirectoryPath();
        string GetAppDataDirectoryPath();
    }
}