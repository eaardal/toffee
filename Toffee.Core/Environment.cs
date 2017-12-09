namespace Toffee
{
    class Environment : IEnvironment
    {
        public string GetAppDataDirectoryPath()
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
        }
    }
}