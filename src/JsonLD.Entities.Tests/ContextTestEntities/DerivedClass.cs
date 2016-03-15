using Newtonsoft.Json;

namespace JsonLD.Entities.Tests.ContextTestEntities
{
    public class DerivedClass : BaseClass
    {
        [JsonProperty("foaf:name")]
        public string Name { get; set; }
    }
}