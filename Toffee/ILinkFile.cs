using System.Collections.Generic;

namespace Toffee
{
    public interface ILinkFile
    {
        void WriteReplacedDlls(string linkName, Dictionary<string, string> replacedDlls, string csprojPath);
    }
}
