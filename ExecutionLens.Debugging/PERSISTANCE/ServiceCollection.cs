using ExecutionLens.Debugging.PERSISTANCE.Contracts;
using ExecutionLens.Debugging.PERSISTANCE.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace ExecutionLens.Debugging.PERSISTANCE;

public static partial class ServiceCollection
{
    public static void AddElasticClient(this IServiceCollection services, IElasticClient client, string index)
    {
        services.AddSingleton<ILogRepository>(new ElasticSearch(client, index));
    }
}