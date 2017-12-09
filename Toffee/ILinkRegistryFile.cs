using System.Collections.Generic;

namespace Toffee
{
    public interface ILinkRegistryFile
    {
        void InsertOrUpdateLink(string linkName, string path);
        (bool exists, Link link) TryGetLink(string linkName);
        IEnumerable<Link> GetAllLinks();
        Link GetLink(string linkName);
    }
}