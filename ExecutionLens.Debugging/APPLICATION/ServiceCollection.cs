using ExecutionLens.Debugging.APPLICATION.Contracts;
using ExecutionLens.Debugging.APPLICATION.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace ExecutionLens.Debugging;

public static partial class ServiceCollection
{
    public static void AddDebugger(this IServiceCollection services, string elasticUri, string index = "logs")
    {
        services.AddSingleton<IElasticClient>(provider =>
        {
            var settings = new ConnectionSettings(new Uri(elasticUri))
                               .DefaultIndex(index);

            return new ElasticClient(settings);
        });

        services.AddScoped<ILogRepository, ElasticRepository>();
        services.AddScoped<IReplayService, ReplayService>();
        services.AddScoped<IReflectionService, ReflectionService>();
    }
}