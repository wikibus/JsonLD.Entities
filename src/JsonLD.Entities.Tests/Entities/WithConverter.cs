using System;
using Newtonsoft.Json;

namespace JsonLD.Entities.Tests.Entities
{
    [JsonConverter(typeof(DummyConverter))]
    public class WithConverter
    {
        public WithConverter(string id)
        {
            this.Id = id;
        }

        public string Id { get; private set; }

        public class DummyConverter : JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                reader.Skip();

                return new WithConverter("TheIdentifier");
            }

            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }
        }
    }
}
