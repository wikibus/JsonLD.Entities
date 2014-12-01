using System;
using Newtonsoft.Json;

namespace JsonLD.Entities
{
    /// <summary>
    /// Converter, which ensures that Uris are serialized as strings
    /// </summary>
    public class StringUriConverter : JsonConverter
    {
        /// <summary>
        /// Writes the JSON representation of the Uri.
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        /// <summary>
        /// Reads the JSON representation of the Uri.
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return new Uri(reader.ReadAsString(), UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Uri);
        }
    }
}
