namespace Toffee.Core.Infrastructure
{
    public class BuildConfiguration
    {
        public static bool IsDebug()
        {
#if DEBUG
            return true;
#endif
            return false;
        }
    }
}
