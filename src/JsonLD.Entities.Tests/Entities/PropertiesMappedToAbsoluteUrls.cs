using Newtonsoft.Json;

namespace JsonLD.Entities.Tests.Entities
{
    public class PropertiesMappedToAbsoluteUrls
    {
        [JsonProperty("http://xmlns.com/foaf/0.1/givenName")]
        public string Name { get; set; }
    }
}