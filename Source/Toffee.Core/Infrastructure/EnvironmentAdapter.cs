namespace Toffee.Core.Infrastructure
{
    class EnvironmentAdapter : IEnvironmentAdapter
    {
        public string GetProgramDataDirectoryPath()
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
        }

        public string GetAppDataDirectoryPath()
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
        }
    }
}