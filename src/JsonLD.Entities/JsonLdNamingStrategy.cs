using Newtonsoft.Json.Serialization;

namespace JsonLD.Entities
{
    /// <summary>
    /// Ensures that JSON-LD keywords are serialized correctly
    /// </summary>
    public class JsonLdNamingStrategy : CamelCaseNamingStrategy
    {
        /// <summary>
        /// Resolves the name of the property.
        /// </summary>
        protected override string ResolvePropertyName(string propertyName)
        {
            var keyword = JsonLdKeywords.GetKeywordForProperty(propertyName);
            if (keyword != null)
            {
                return keyword;
            }

            return base.ResolvePropertyName(propertyName);
        }
    }
}