using Nest;
using ExecutionLens.Debugging.DOMAIN.Models;
using ExecutionLens.Debugging.APPLICATION.Contracts;

namespace ExecutionLens.Debugging.APPLICATION.Implementations;

internal class ElasticRepository(IElasticClient _elasticClient) : ILogRepository
{
    public async Task<MethodLog?> Get(string id)
    {
        var result = await _elasticClient.GetAsync<MethodLog>(id);

        if (!result.Found)
        {
            return null;
        }

        var node = result.Source;

        bool isRoot = node.NodePath is null;

        if (isRoot)
        {
            await GetInteractions(node, id);
            return node;
        }
        else
        {
            string rootId = node.NodePath!.Split('/').First();

            var rootResult = await _elasticClient.GetAsync<MethodLog>(rootId);
            var root = rootResult.Source;

            await GetInteractions(root, rootResult.Id);
            return root;
        }
    }

    private async Task GetInteractions(MethodLog parent, string path)
    {
        var searchResponse = await _elasticClient.SearchAsync<MethodLog>(s => s
            .Query(q => q
                .Term(t => t.Field(f => f.NodePath.Suffix("keyword")).Value(path))
                )
            );

        if (searchResponse.Documents is not null)
        {
            foreach (var document in searchResponse.Hits)
            {
                parent.Interactions.Add(document.Source);

                await GetInteractions(document.Source, $"{path}/{document.Id}");
            }
        }
    }
}
