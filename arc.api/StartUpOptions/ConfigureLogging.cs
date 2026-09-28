using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace arc.api.StartUpOptions;

public static class ConfigureLogging
{
    public static IServiceCollection AddArcLogging(this IServiceCollection services)
    {
        services.AddSingleton(typeof(ILogger), sp =>
        {
            var factory = sp.GetRequiredService<ILoggerFactory>();
            return factory.CreateLogger("arc.app.Common.LogWriter");
        });
        return services;
    }
}
