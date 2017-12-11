namespace Toffee.Core
{
    public class LinkFromCommandArgs
    {
        public string SourceDirectoryPath { get; }
        public string LinkName { get; }

        public LinkFromCommandArgs(string sourceDirectoryPath, string linkName)
        {
            SourceDirectoryPath = sourceDirectoryPath;
            LinkName = linkName;
        }
    }
}