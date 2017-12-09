using Peon.Infrastructure.DependencyInjection;

namespace Toffee.Infrastructure.Startup
{
    public class Bootstrapper
    {
        public static TinyIoCContainer Wire()
        {
            var ioc = TinyIoCContainer.Current;
            ioc.AutoRegister();

            ioc.RegisterMultiple<ICommand>(new[]
            {
                typeof(LinkFromCommand),
            });

            ioc.Register<ICommandArgsParser<LinkFromCommandArgs>, LinkFromCommandArgsParser>();
            
            return TinyIoCContainer.Current;
        }
    }
}
