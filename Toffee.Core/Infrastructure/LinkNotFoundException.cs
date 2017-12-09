using System;

namespace Toffee.Infrastructure
{
    internal class LinkNotFoundException : Exception
    {
        public LinkNotFoundException(string message) : base(message)
        {
            
        }
    }
}