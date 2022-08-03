using OrchardCore.Contents.Indexing;

namespace OrchardCore.Search.Elasticsearch.Model
{
    public class ElasticsearchSettings
    {
        public static readonly string[] FullTextField = new string[] { IndexingConstants.FullTextKey };

        public static string StandardAnalyzer = "standardanalyzer";

        public string SearchIndex { get; set; }

        public string[] DefaultSearchFields { get; set; } = FullTextField;

        public bool AllowElasticQueryStringQueryInSearch { get; set; } = false;
    }
}