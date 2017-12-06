using System;
using System.Collections.Generic;
using Toffee.Core;

namespace Toffee.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            /*  toffee link-from={path-bin-debug} as={link-name} - Lagrer path til bin/debug mappen i prosjektet man vil linke til
             *    
             *      1. Lagre path til bin/debug i en fil
             *      
             *  toffee link-to={path-packages} from={link-name} using={dlls}
             *  
             *      1. Hente path til bin/debug fra fil, basert på link-name
             *      2. Lage symlink for mappen dll'en ligger under, under packages/ mappen i solution
             *      3. Symlinke til bin/debug
             */

            var commands = new List<ICommand>();
        }
    }
}
