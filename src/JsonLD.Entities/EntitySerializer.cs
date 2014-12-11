using System.Linq;
using JsonLD.Core;
using JsonLD.Entities.Converters;
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
        private readonly IFrameProvider _frameProvider;
        private readonly JsonSerializer _jsonSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySerializer"/> class.
        /// </summary>
        /// <param name="contextProvider">The JSON-LD @context provider.</param>
        public EntitySerializer(IContextProvider contextProvider)
            : this(contextProvider, new NullFrameProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySerializer"/> class.
        /// </summary>
        /// <param name="contextProvider">The JSON-LD @context provider.</param>
        /// <param name="frameProvider">The JSON-LD frame provider.</param>
        public EntitySerializer(IContextProvider contextProvider, IFrameProvider frameProvider)
        {
            _contextProvider = contextProvider;
            _frameProvider = frameProvider;
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
            return Deserialize<T>(jsonLdObject);
        }

        /// <summary>
        /// Deserializes the JSON-LD object into a typed model.
        /// </summary>
        /// <typeparam name="T">destination entity model</typeparam>
        /// <param name="jsonLd">a JSON-LD object</param>
        public T Deserialize<T>(JToken jsonLd)
        {
            var jsonLdContext = _contextProvider.GetContext(typeof(T));
            if (jsonLdContext == null)
            {
                throw new ContextNotFoundException(typeof(T));
            }

            var frame = _frameProvider.GetFrame(typeof(T));
            if (frame == null)
            {
                return JsonLdProcessor.Compact(jsonLd, jsonLdContext, new JsonLdOptions()).ToObject<T>(_jsonSerializer);
            }

            frame["@context"] = jsonLdContext;
            var framed = JsonLdProcessor.Frame(jsonLd, frame, new JsonLdOptions());
            return framed["@graph"].Single().ToObject<T>(_jsonSerializer);
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
