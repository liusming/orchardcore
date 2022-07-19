using OrchardCore.Indexing;

namespace OrchardCore.Lucene.Model
{
    /// <summary>
    /// Represents the indexing settings for a content part or a field.
    /// </summary>
    public class LuceneContentIndexSettings : IContentIndexSettings
    {
        public bool Included { get; set; }
        public bool Stored { get; set; }
        public bool Analyzed { get; set; }

        public DocumentIndexOptions ToOptions()
        {
            var options = DocumentIndexOptions.None;

            if (Stored)
            {
                options |= DocumentIndexOptions.Store;
            }

            if (Analyzed)
            {
                options |= DocumentIndexOptions.Analyze;
            }

            return options;
        }
    }
}

