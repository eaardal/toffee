using System.Collections.Generic;

namespace Toffee
{
    public class LinkMap
    {
        public Link Link { get; }
        public IReadOnlyCollection<ReplacementRecord> ReplacementRecords { get; }

        public LinkMap(Link link, IReadOnlyCollection<ReplacementRecord> replacementRecords)
        {
            Link = link;
            ReplacementRecords = replacementRecords;
        }
    }
}
