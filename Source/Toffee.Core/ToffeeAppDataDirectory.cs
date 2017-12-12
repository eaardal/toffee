using System.IO;
using Toffee.Core.Infrastructure;

namespace Toffee.Core
{
    class ToffeeAppDataDirectory : IToffeeAppDataDirectory
    {
        private readonly IEnvironmentAdapter _environment;
        private readonly IFilesystem _filesystem;

        public ToffeeAppDataDirectory(IEnvironmentAdapter environment, IFilesystem filesystem)
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
