using System;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities.Context
{
    /// <summary>
    /// Used to create JSON-LD @context
    /// </summary>
    public static class Vocab
    {
        /// <summary>
        /// Creates a property JSON-LD @vocab
        /// </summary>
        public static JProperty Is(string baseUri)
        {
            return new JProperty(JsonLdKeywords.Vocab, baseUri);
        }

        /// <summary>
        /// Creates a property JSON-LD @vocab
        /// </summary>
        public static JProperty Is(Uri baseUri)
        {
            return new JProperty(JsonLdKeywords.Vocab, baseUri);
        }
    }
}
