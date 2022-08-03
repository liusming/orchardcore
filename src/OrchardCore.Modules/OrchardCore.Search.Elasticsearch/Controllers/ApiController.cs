using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrchardCore.Search.Elasticsearch.Model;

namespace OrchardCore.Search.Elasticsearch
{
    [Route("api/elasticsearch")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Api"), IgnoreAntiforgeryToken, AllowAnonymous]
    public class ApiController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ElasticsearchQuerySource _elasticQuerySource;

        public ApiController(
            IAuthorizationService authorizationService,
            ElasticsearchQuerySource elasticQuerySource)
        {
            _authorizationService = authorizationService;
            _elasticQuerySource = elasticQuerySource;
        }

        [HttpGet]
        [Route("content")]
        public async Task<IActionResult> Content([FromQuery] ElasticsearchQueryModel queryModel)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.QueryElasticApi))
            {
                return this.ChallengeOrForbid("Api");
            }

            var result = await ElasticQueryApiAsync(queryModel, returnContentItems: true);

            return new ObjectResult(result);
        }

        [HttpPost]
        [Route("content")]
        public async Task<IActionResult> ContentPost(ElasticsearchQueryModel queryModel)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.QueryElasticApi))
            {
                return this.ChallengeOrForbid();
            }

            var result = await ElasticQueryApiAsync(queryModel, returnContentItems: true);

            return new ObjectResult(result);
        }

        [HttpGet]
        [Route("documents")]
        public async Task<IActionResult> Documents([FromQuery] ElasticsearchQueryModel queryModel)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.QueryElasticApi))
            {
                return this.ChallengeOrForbid();
            }

            var result = await ElasticQueryApiAsync(queryModel);

            return new ObjectResult(result);
        }

        [HttpPost]
        [Route("documents")]
        public async Task<IActionResult> DocumentsPost(ElasticsearchQueryModel queryModel)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.QueryElasticApi))
            {
                return this.ChallengeOrForbid("Api");
            }

            var result = await ElasticQueryApiAsync(queryModel);

            return new ObjectResult(result);
        }

        private Task<Queries.IQueryResults> ElasticQueryApiAsync(ElasticsearchQueryModel queryModel, bool returnContentItems = false)
        {
            var elasticQuery = new ElasticsearchQuery
            {
                Index = queryModel.IndexName,
                Template = queryModel.Query,
                ReturnContentItems = returnContentItems
            };

            var queryParameters = queryModel.Parameters != null ?
                JsonConvert.DeserializeObject<Dictionary<string, object>>(queryModel.Parameters)
                : new Dictionary<string, object>();

            var result = _elasticQuerySource.ExecuteQueryAsync(elasticQuery, queryParameters);
            return result;
        }
    }
}
