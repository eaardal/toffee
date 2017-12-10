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
                typeof(LinkToCommand),
                typeof(RestoreCommand)
            });

            ioc.Register<ICommandArgsParser<LinkFromCommandArgs>, LinkFromCommandArgsParser>();
            ioc.Register<ICommandArgsParser<LinkToCommandArgs>, LinkToCommandArgsParser>();
            ioc.Register<ICommandArgsParser<RestoreCommandArgs>, RestoreCommandArgsParser>();
            
            return TinyIoCContainer.Current;
        }
    }
}
