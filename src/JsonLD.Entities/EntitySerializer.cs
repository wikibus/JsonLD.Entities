using JsonLD.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities
{
    /// <summary>
    /// Entity serializer
    /// </summary>
    public class EntitySerializer : IEntitySerializer
    {
        private readonly IContextProvider _contextProvider;
        private readonly JsonSerializer _jsonSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySerializer"/> class.
        /// </summary>
        /// <param name="contextProvider">The JSON-LD @context provider.</param>
        public EntitySerializer(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
            _jsonSerializer = new JsonSerializer
                              {
                                  DateFormatHandling = DateFormatHandling.IsoDateFormat,
                                  ContractResolver = new JsonLdContractResolver(_contextProvider)
                              };
        }

        /// <summary>
        /// Deserializes the NQuads into a typed model
        /// </summary>
        /// <typeparam name="T">destination entity model type</typeparam>
        /// <param name="nQuads">RDF data in NQuads.</param>
        public T Deserialize<T>(string nQuads)
        {
            var jsonLdObject = JsonLdProcessor.FromRDF(nQuads);
            var jsonLdContext = _contextProvider.GetContext(typeof(T));
            if (jsonLdContext == null)
            {
                throw new ContextNotFoundException(typeof(T));
            }

            return JsonLdProcessor.Compact(jsonLdObject, jsonLdContext, new JsonLdOptions()).ToObject<T>(_jsonSerializer);
        }

        /// <summary>
        /// Deserializes the JSON-LD object into a typed model.
        /// </summary>
        /// <typeparam name="T">destination entity model</typeparam>
        /// <param name="jsonLd">a JSON-LD object</param>
        public T Deserialize<T>(JObject jsonLd)
        {
            var jsonLdContext = _contextProvider.GetContext(typeof(T));
            if (jsonLdContext == null)
            {
                return jsonLd.ToObject<T>(_jsonSerializer);
            }

            var compacted = JsonLdProcessor.Compact(jsonLd, jsonLdContext, new JsonLdOptions());
            return compacted.ToObject<T>(_jsonSerializer);
        }

        /// <summary>
        /// Serializes the specified entity as JSON-LD.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// A compacted JSON-LD object
        /// </returns>
        public JObject Serialize(object entity)
        {
            return JObject.FromObject(entity, _jsonSerializer);
        }
    }
}
