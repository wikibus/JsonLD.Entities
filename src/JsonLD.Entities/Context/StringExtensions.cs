using System;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities.Context
{
    /// <summary>
    /// Used to create JSON-LD @context
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Creates a prefix definition property for a JSON-LD @context
        /// </summary>
        public static JProperty IsPrefixOf(this string prefix, string ns)
        {
            return new JProperty(prefix, ns);
        }

        /// <summary>
        /// Creates a prefix definition property for a JSON-LD @context
        /// </summary>
        public static JProperty IsPrefixOf(this string prefix, Uri ns)
        {
            return new JProperty(prefix, ns);
        }

        /// <summary>
        /// Creates a property definition for a JSON-LD @context
        /// </summary>
        public static PropertyBuilder IsProperty(this string property, string uriOrPrefixedName)
        {
            return new PropertyBuilder(property, uriOrPrefixedName);
        }

        /// <summary>
        /// Creates a language definition for a JSON-LD @context
        /// </summary>
        public static JProperty IsLanguage(this string languageCode)
        {
            return new JProperty(JsonLdKeywords.Language, languageCode);
        }
    }
}
