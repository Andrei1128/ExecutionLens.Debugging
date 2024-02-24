using Common.PERSISTANCE.Contracts;
using Nest;
using PostMortem.Common.DOMAIN.Models;

namespace Common.PERSISTANCE.Implementations;

internal class ElasticSearch(IElasticClient client, string index) : ILogRepository
{
    private readonly IElasticClient _client = client;

    public async Task<string> Add(MethodLog log) => (await _client.IndexAsync(log, i => i.Index(index))).Id;
    public async Task<MethodLog> Get(string id) => (await _client.GetAsync<MethodLog>(id, g => g.Index(index))).Source;
}
