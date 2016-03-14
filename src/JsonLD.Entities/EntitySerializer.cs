using System.Linq;
using JsonLD.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NullGuard;

namespace JsonLD.Entities
{
    /// <summary>
    /// Entity serializer
    /// </summary>
    public class EntitySerializer : IEntitySerializer
    {
        private readonly ContextResolver _contextResolver;
        private readonly IFrameProvider _frameProvider;
        private readonly JsonSerializer _jsonSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySerializer"/> class.
        /// </summary>
        public EntitySerializer() : this(new ContextResolver(new NullContextProvider()))
        {
        }

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
            : this(new ContextResolver(contextProvider))
        {
            _frameProvider = frameProvider;
        }

        private EntitySerializer(ContextResolver contextResolver)
        {
            _contextResolver = contextResolver;
            _jsonSerializer = new JsonLdSerializer();
        }

        /// <summary>
        /// Deserializes the NQuads into a typed model
        /// </summary>
        /// <typeparam name="T">destination entity model type</typeparam>
        /// <param name="nQuads">RDF data in NQuads.</param>
        public T Deserialize<T>(string nQuads)
        {
            var jsonLd = JsonLdProcessor.FromRDF(nQuads, new NQuadsParser());
            var context = _contextResolver.GetContext(typeof(T));
            var frame = _frameProvider.GetFrame(typeof(T));
            if (context == null)
            {
                throw new ContextNotFoundException(typeof(T));
            }

            return Deserialize<T>(jsonLd, context, frame);
        }

        /// <summary>
        /// Deserializes the JSON-LD object into a typed model.
        /// </summary>
        /// <typeparam name="T">destination entity model</typeparam>
        /// <param name="jsonLd">a JSON-LD object</param>
        public T Deserialize<T>(JToken jsonLd)
        {
            var jsonLdContext = _contextResolver.GetContext(typeof(T));
            var frame = _frameProvider.GetFrame(typeof(T));

            return Deserialize<T>(jsonLd, jsonLdContext, frame);
        }

        /// <summary>
        /// Serializes the specified entity as JSON-LD.
        /// </summary>
        /// <returns>
        /// A compacted JSON-LD object
        /// </returns>
        public JObject Serialize(object entity, [AllowNull] SerializationOptions options = null)
        {
            options = options ?? new SerializationOptions();
            var jsonLd = JObject.FromObject(entity, _jsonSerializer);

            var context = _contextResolver.GetContext(entity);
            if (context != null && IsNotEmpty(context))
            {
                jsonLd.AddFirst(new JProperty("@context", context));

                if (options.SerializeCompacted)
                {
                    jsonLd = JsonLdProcessor.Compact(jsonLd, context, new JsonLdOptions());
                }
            }

            return jsonLd;
        }

        private static bool IsNotEmpty(JToken context)
        {
            if (context is JObject)
            {
                return ((JObject)context).Count > 0;
            }

            var array = context as JArray;
            if (array != null)
            {
                return array.All(IsNotEmpty);
            }

            return true;
        }

        private T Deserialize<T>(JToken jsonLd, JToken context, JObject frame)
        {
            if (context == null)
            {
                return jsonLd.ToObject<T>(_jsonSerializer);
            }

            if (frame == null)
            {
                return JsonLdProcessor.Compact(jsonLd, context, new JsonLdOptions()).ToObject<T>(_jsonSerializer);
            }

            frame["@context"] = context;
            var framed = JsonLdProcessor.Frame(jsonLd, frame, new JsonLdOptions());
            return framed["@graph"].Single().ToObject<T>(_jsonSerializer);
        }
    }
}
