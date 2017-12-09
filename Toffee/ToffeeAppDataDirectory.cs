using System.IO;
using Toffee.Infrastructure;

namespace Toffee
{
    class ToffeeAppDataDirectory : IToffeeAppDataDirectory
    {
        private readonly IEnvironment _environment;
        private readonly IFilesystem _filesystem;

        public ToffeeAppDataDirectory(IEnvironment environment, IFilesystem filesystem)
        {
            _environment = environment;
            _filesystem = filesystem;
        }

        public string EnsureExists()
        {
            var appDataDirectory = _environment.GetAppDataDirectoryPath();
            var toffeeAppDataDirectory = Path.Combine(appDataDirectory, "Toffee");

            if (!_filesystem.DirectoryExists(toffeeAppDataDirectory))
            {
                _filesystem.CreateDirectory(toffeeAppDataDirectory);
            }

            return toffeeAppDataDirectory;
        }
    }
}
