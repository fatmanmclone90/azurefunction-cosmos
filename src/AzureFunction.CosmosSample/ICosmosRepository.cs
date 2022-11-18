using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AzureFunction.CosmosSample
{
    public interface ICosmosRepository
    {
        Task<ArticleIngested> AddAsync(ArticleIngested item, CancellationToken cancellationToken);

        Task AddManyAsync(List<ArticleIngested> items, CancellationToken cancellationToken);
    }
}