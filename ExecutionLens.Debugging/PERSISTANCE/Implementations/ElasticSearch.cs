using Nest;
using ExecutionLens.Debugging.DOMAIN.Models;
using ExecutionLens.Debugging.PERSISTANCE.Contracts;

namespace ExecutionLens.Debugging.PERSISTANCE.Implementations;

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

    private async Task IndexChildrens(List<MethodLog>? childrens, string path)
    {
        if (childrens is null)
            return;

        foreach (var child in childrens)
        {
            child.NodePath = path;

            var indexResponse = await _client.IndexAsync(child, idx => idx.Index(index));

            await IndexChildrens(child.Interactions, $"{path}/{indexResponse.Id}");
        }
    }

    public async Task<MethodLog> Get(string id) => (await _client.GetAsync<MethodLog>(id, g => g.Index(index))).Source;
}
