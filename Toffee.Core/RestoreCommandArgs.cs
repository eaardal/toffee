namespace Toffee.Core
{
    public class RestoreCommandArgs
    {
        public string DestinationDirectoryPath { get; }

        public RestoreCommandArgs(string destinationDirectoryPath)
        {
            DestinationDirectoryPath = destinationDirectoryPath;
        }
    }
}