namespace Toffee.Infrastructure
{
    public interface IEnvironmentAdapter
    {
        string GetProgramDataDirectoryPath();
        string GetAppDataDirectoryPath();
    }
}