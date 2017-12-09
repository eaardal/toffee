using System.Collections.Generic;

namespace Toffee
{
    public interface ILinkRegistryFile
    {
        void SaveOrUpdateLink(string linkName, string path);
        (bool ok, Link link) TryGetLink(string linkName);
        IEnumerable<Link> GetAllLinks();
    }
}