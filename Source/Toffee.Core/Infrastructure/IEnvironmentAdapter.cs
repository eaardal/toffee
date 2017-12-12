namespace Toffee.Core.Infrastructure
{
    public interface IEnvironmentAdapter
    {
        string GetProgramDataDirectoryPath();
        string GetAppDataDirectoryPath();
    }
}