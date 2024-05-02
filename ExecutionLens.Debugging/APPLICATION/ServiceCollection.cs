using ExecutionLens.Debugging.APPLICATION.Contracts;
using ExecutionLens.Debugging.APPLICATION.Implementations;
using ExecutionLens.Debugging.DOMAIN.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;

namespace ExecutionLens.Debugging;

public static partial class ServiceCollection
{
    public static void AddDebugger(this IServiceCollection services, Action<DebuggerConfiguration> configuration)
    {
        services.Configure(configuration);

        services.AddSingleton<IElasticClient>(provider =>
        {
            var options = provider.GetRequiredService<IOptionsMonitor<DebuggerConfiguration>>();
            var config = options.CurrentValue;

            var settings = new ConnectionSettings(new Uri(config.ElasticUri))
                               .DefaultIndex(config.Index);

            return new ElasticClient(settings);
        });

        services.AddScoped<ILogRepository, ElasticRepository>();
        services.AddScoped<IReplayService, ReplayService>();
        services.AddScoped<IReflectionService, ReflectionService>();
    }
}