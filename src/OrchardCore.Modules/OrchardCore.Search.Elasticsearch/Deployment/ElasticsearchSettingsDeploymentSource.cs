using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OrchardCore.Deployment;

namespace OrchardCore.Search.Elasticsearch.Deployment
{
    public class ElasticsearchSettingsDeploymentSource : IDeploymentSource
    {
        private readonly ElasticsearchIndexingService _elasticIndexingService;

        public ElasticsearchSettingsDeploymentSource(ElasticsearchIndexingService elasticIndexingService)
        {
            _elasticIndexingService = elasticIndexingService;
        }

        public async Task ProcessDeploymentStepAsync(DeploymentStep step, DeploymentPlanResult result)
        {
            var elasticSettingsStep = step as ElasticsearchSettingsDeploymentStep;

            if (elasticSettingsStep == null)
            {
                return;
            }

            var elasticSettings = await _elasticIndexingService.GetElasticSettingsAsync();

            // Adding Elasticsearch settings
            result.Steps.Add(new JObject(
                new JProperty("name", "Settings"),
                new JProperty("ElasticsearchSettings", JObject.FromObject(elasticSettings))
            ));
        }
    }
}