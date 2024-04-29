using Nest;
using ExecutionLens.Debugging.DOMAIN.Models;
using ExecutionLens.Debugging.APPLICATION.Contracts;

namespace ExecutionLens.Debugging.APPLICATION.Implementations;

internal class ElasticRepository(IElasticClient _elasticClient) : ILogRepository
{
    public async Task<MethodLog> Get(string id) => (await _elasticClient.GetAsync<MethodLog>(id)).Source;
}
