using System;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities.Context
{
    /// <summary>
    /// Used to create JSON-LD @context
    /// </summary>
    public static class Base
    {
        /// <summary>
        /// Creates a property JSON-LD @base
        /// </summary>
        public static JProperty Is(string baseUri)
        {
            return new JProperty(JsonLdKeywords.Base, baseUri);
        }

        /// <summary>
        /// Creates a property JSON-LD @base
        /// </summary>
        public static JProperty Is(Uri baseUri)
        {
            return new JProperty(JsonLdKeywords.Base, baseUri);
        }
    }
}
