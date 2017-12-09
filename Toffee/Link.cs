namespace Toffee
{
    public class Link
    {
        public string LinkName { get; }
        public string SourceDirectoryPath { get; }

        public Link(string linkName, string sourceDirectoryPath)
        {
            LinkName = linkName;
            SourceDirectoryPath = sourceDirectoryPath;
        }

        public static Link ParseFromCsv(string arg)
        {
            var parts = arg.Split(',');
            return new Link(parts[0], parts[1]);
        }
    }
}