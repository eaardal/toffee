namespace Toffee
{
    public class ReplacementRecord
    {
        public string Before { get; }
        public string After { get; }

        public ReplacementRecord(string before, string after)
        {
            Before = before;
            After = after;
        }
    }
}
