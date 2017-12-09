﻿using System.Collections.Generic;

namespace Toffee
{
    public class LinkToCommandArgs
    {
        public string DestinationDirectoryPath { get; }
        public string LinkName { get; }
        public IEnumerable<string> Dlls { get; }

        public LinkToCommandArgs(string destinationDirectoryPath, string linkName, IEnumerable<string> dlls)
        {
            DestinationDirectoryPath = destinationDirectoryPath;
            LinkName = linkName;
            Dlls = dlls;
        }
    }
}