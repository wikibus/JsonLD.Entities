using Newtonsoft.Json;

namespace JsonLD.Entities
{
    /// <summary>
    /// A serializer set-up to deserialize valid JSON-LD
    /// </summary>
    public sealed class JsonLdSerializer : JsonSerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLdSerializer"/> class.
        /// </summary>
        public JsonLdSerializer()
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat;
            ContractResolver = new JsonLdContractResolver();
            NullValueHandling = NullValueHandling.Ignore;
        }
    }
}
