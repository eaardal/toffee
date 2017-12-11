using System.Collections.Generic;

namespace Toffee.Core
{
    public class HelpText
    {
        public string Command { get; private set; }
        public string Description { get; private set; }
        public Dictionary<string, string> Arguments { get; } = new Dictionary<string, string>();
        public List<string> Examples { get; } = new List<string>();

        public HelpText WithCommand(string command)
        {
            Command = command;
            return this;
        }

        public HelpText WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public HelpText WithArgument(string argName, string argDescription)
        {
            Arguments.Add(argName, argDescription);
            return this;
        }

        public HelpText WithExample(string example)
        {
            Examples.Add(example);
            return this;
        }
    }
}