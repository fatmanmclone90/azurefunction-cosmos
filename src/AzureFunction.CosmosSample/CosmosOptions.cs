using System.ComponentModel.DataAnnotations;

namespace AzureFunction.CosmosSample
{
    public class CosmosOptions
    {

        [Required]
        public string ConnectionString { get; set; }

        [Required]
        public string DatabaseId { get; set; }

        [Required]
        public string Container { get; set; }
    }
}
