using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoBogus;

namespace AzureFunction.CosmosSample
{
    public class FakeArticleIngestedFunction
    {
        private readonly ILogger<FakeArticleIngestedFunction> _logger;
        private readonly ICosmosRepository _cosmosRepository;

        public FakeArticleIngestedFunction(
            ILogger<FakeArticleIngestedFunction> logger, 
            ICosmosRepository cosmosRepository)
        {
            _logger = logger;
            _cosmosRepository = cosmosRepository;
        }

        [Function(nameof(FakeArticleIngestedFunction))]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData request,
            FunctionContext executionContext)
        {
            await _cosmosRepository.AddManyAsync(
                GenerateManyArticleIngested(50), 
                executionContext.CancellationToken);
            
            _logger.LogInformation("Created Items");

            var response = request.CreateResponse(HttpStatusCode.Created);

            return response;
        }

        private static List<ArticleIngested> GenerateManyArticleIngested(int count) => 
            new AutoFaker<ArticleIngested>().Generate(count);
    }
}