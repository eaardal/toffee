using Peon.Infrastructure.DependencyInjection;

namespace Toffee.Infrastructure.DependencyInjection
{
    public static class IoC
    {
        public static T Resolve<T>() where T : class
        {
            return TinyIoCContainer.Current.Resolve<T>();
        }
    }
}
