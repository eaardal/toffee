using System.Collections.Generic;

namespace Toffee.Core
{
    public class RestoreCommandArgs
    {
        public bool RestoreAllLinks { get; }
        public IReadOnlyCollection<string> LinksToRestore { get; }
        public string DestinationDirectoryPath { get; }

        public RestoreCommandArgs(string destinationDirectoryPath, IReadOnlyCollection<string> linksToRestore, bool restoreAllLinks)
        {
            DestinationDirectoryPath = destinationDirectoryPath;
            RestoreAllLinks = restoreAllLinks;
            LinksToRestore = linksToRestore;
        }
    }
}