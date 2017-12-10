using System;
using System.IO;
using System.Threading;

namespace Toffee.Infrastructure
{
    class Retry
    {
        public static T Operation<T>(Func<T> action, int retries = 3, int retryDelay = 1000)
        {
            var lastEx = new IOException($"Operation did not complete successfully after {retries} retries");

            for (var i = 1; i <= retries; ++i)
            {
                try
                {
                    return action();
                }
                catch (IOException ex) when (i <= retries)
                {
                    lastEx = ex;
                    Thread.Sleep(retryDelay);
                }
            }

            throw lastEx;
        }

        public static void Operation(Action action, int retries = 3, int retryDelay = 1000)
        {
            for (var i = 1; i <= retries; ++i)
            {
                try
                {
                    action();
                    break;
                }
                catch (IOException) when (i <= retries)
                {
                    Thread.Sleep(retryDelay);
                }
            }
        }
    }
}
