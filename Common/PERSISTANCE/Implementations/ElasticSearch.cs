using Common.DOMAIN.Models;
using Common.PERSISTANCE.Contracts;
using Elasticsearch.Net;
using Nest;
using PostMortem.Common.DOMAIN.Models;

namespace Common.PERSISTANCE.Implementations;

internal class ElasticSearch(IElasticClient client, string index) : ILogRepository
{
    private readonly IElasticClient _client = client;

    public async Task<string> Add(MethodLog log)
    {
        var indexResponse = await _client.IndexAsync(log, idx => idx.Index(index));

        var rootId = indexResponse.Id;

        await IndexChildrens(log.Interactions, rootId);

        return rootId;
    }

    private async Task IndexChildrens(List<MethodLog>? childrens, string parentId)
    {
        if (childrens is null)
            return;

        foreach (var child in childrens)
        {
            var indexResponse = await _client.IndexAsync(child, idx => idx.Index(index));

            ClosureEntry closure = new()
            {
                ParentId = parentId,
                ChildId = indexResponse.Id
            };

            await _client.IndexAsync(closure, idx => idx.Index($"{index}_closure"));

            await IndexChildrens(child.Interactions, indexResponse.Id);
        }
    }

    public async Task<MethodLog> Get(string id) => (await _client.GetAsync<MethodLog>(id, g => g.Index(index))).Source;
}
