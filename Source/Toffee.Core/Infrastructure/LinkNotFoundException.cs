using System;

namespace Toffee.Core.Infrastructure
{
    internal class LinkNotFoundException : Exception
    {
        public LinkNotFoundException(string message) : base(message)
        {
            
        }
    }
}