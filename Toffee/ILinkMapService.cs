namespace Toffee
{
    public interface ILinkMapService
    {
        LinkMap GetLinkMap(string linkName, string destinationDirectoryPath);
    }
}