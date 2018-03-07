using Serilog;
using Toffee.Core.Infrastructure.DependencyInjection;

namespace Toffee.Core.Infrastructure.Startup
{
    public class Bootstrapper
    {
        public static TinyIoCContainer Wire()
        {
            var loggerConfiguration = new LoggerConfiguration();

            loggerConfiguration
                .MinimumLevel.Information()
                .WriteTo.RollingFile(@"Logs\Log-{Date}.txt");

            if (BuildConfiguration.IsDebug())
            {
                loggerConfiguration.WriteTo.Console().MinimumLevel.Debug();
            }

            Log.Logger = loggerConfiguration.CreateLogger();

            var ioc = TinyIoCContainer.Current;
            ioc.AutoRegister();

            ioc.RegisterMultiple<ICommand>(new[]
            {
                typeof(LinkFromCommand),
                typeof(LinkToCommand),
                typeof(RestoreCommand),
                typeof(GetLinksCommand),
                typeof(HelpCommand)
            });

            ioc.Register<ICommandArgsParser<LinkFromCommandArgs>, LinkFromCommandArgsParser>();
            ioc.Register<ICommandArgsParser<LinkToCommandArgs>, LinkToCommandArgsParser>();
            ioc.Register<ICommandArgsParser<RestoreCommandArgs>, RestoreCommandArgsParser>();

            ioc.Register(Log.Logger);
            
            return TinyIoCContainer.Current;
        }
    }
}
