using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace AzureFunction.CosmosSample
{
    public class CosmosRepository : ICosmosRepository
    {
        private readonly CosmosClient _cosmosClient;
        private readonly CosmosOptions _options;
        private readonly Container _container;

        public CosmosRepository(CosmosClient cosmosClient, IOptions<CosmosOptions> options)
        {
            _cosmosClient = cosmosClient;
            _options = options.Value;
            _container = _cosmosClient.GetContainer(_options.DatabaseId, _options.Container);
        }

        public async Task<ArticleIngested> AddAsync(
            ArticleIngested item, 
            CancellationToken cancellationToken) =>
            await _container.CreateItemAsync(
                item: item,
                partitionKey: new PartitionKey(item.id.ToString()),
                cancellationToken: cancellationToken);

        public async Task AddManyAsync(
            List<ArticleIngested> items, 
            CancellationToken cancellationToken)
        {
            var concurrentTasks = 
                items.Select(itemToInsert => _container.CreateItemAsync(
                    itemToInsert, 
                    new PartitionKey(itemToInsert.id.ToString()), 
                    cancellationToken: cancellationToken))
                    .ToList();

            await Task.WhenAll(concurrentTasks);
        }
    }
}