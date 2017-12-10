namespace Toffee
{
    public class LinkMapService : ILinkMapService
    {
        private readonly ILinkRegistryFile _linkRegistryFile;
        private readonly ILinkMapFile _linkFile;

        public LinkMapService(ILinkRegistryFile linkRegistryFile, ILinkMapFile linkFile)
        {
            _linkRegistryFile = linkRegistryFile;
            _linkFile = linkFile;
        }

        public LinkMap GetLinkMap(string linkName, string destinationDirectoryPath)
        {
            var link = _linkRegistryFile.GetLink(linkName);

            var replacementRecords = _linkFile.GetReplacementRecords(linkName, destinationDirectoryPath);

            return new LinkMap(link, replacementRecords);
        }
    }
}
