using System;
using Newtonsoft.Json;
using NullGuard;

namespace JsonLD.Entities.Converters
{
    /// <summary>
    /// Converter, which ensures that Uris are serialized as strings
    /// </summary>
    public class StringUriConverter : JsonConverter
    {
        /// <summary>
        /// Writes the JSON representation of the Uri.
        /// </summary>
        public override void WriteJson(JsonWriter writer, [AllowNull] object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        /// <summary>
        /// Reads the JSON representation of the Uri.
        /// </summary>
        [return: AllowNull]
        public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
        {
            return new Uri(reader.Value.ToString(), UriKind.RelativeOrAbsolute);
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
