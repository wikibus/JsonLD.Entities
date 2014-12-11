using Newtonsoft.Json.Linq;

namespace JsonLD.Entities
{
    /// <summary>
    /// Contract for serializing and de-serializing RDF resources
    /// </summary>
    public interface IEntitySerializer
    {
        /// <summary>
        /// Deserializes the NQuads into a typed entity
        /// </summary>
        /// <typeparam name="T">destination entity model type</typeparam>
        /// <param name="nQuads">RDF data in NQuads.</param>
        T Deserialize<T>(string nQuads);

        /// <summary>
        /// Deserializes the JSON-LD object into a typed entity
        /// </summary>
        /// <typeparam name="T">destination entity model</typeparam>
        /// <param name="jsonLd">a JSON-LD object</param>
        T Deserialize<T>(JToken jsonLd);

        /// <summary>
        /// Serializes the specified entity as JSON-LD.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A compacted JSON-LD object</returns>
        JObject Serialize(object entity);
    }
}
