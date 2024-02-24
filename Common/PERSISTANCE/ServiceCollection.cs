using Common.PERSISTANCE.Contracts;
using Common.PERSISTANCE.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace PostMortem.Common;

public static partial class ServiceCollection
{
    public static void AddElasticClient(this IServiceCollection services, IElasticClient client, string index)
    {
        services.AddSingleton<ILogRepository>(new ElasticSearch(client, index));
    }
}