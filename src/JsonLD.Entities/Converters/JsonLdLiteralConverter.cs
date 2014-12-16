using System;
using Newtonsoft.Json;
using NullGuard;

namespace JsonLD.Entities.Converters
{
    /// <summary>
    /// pending doc
    /// </summary>
    public class JsonLdLiteralConverter : JsonConverter
    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, [AllowNull] object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartObject)
            {
                return DeserializeLiteral(reader, objectType, serializer);
            }

            object value = null;
            while (reader.TokenType != JsonToken.EndObject)
            {
                reader.Read();

                if (reader.TokenType == JsonToken.PropertyName && Equals(reader.Value, "@value"))
                {
                    reader.Read();
                    value = DeserializeLiteral(reader, objectType, serializer);
                }
                else
                {
                    reader.Skip();
                }
            }

            return value;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        ///   <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        /// <summary>
        /// When implemented in derived classes can be used to customize deserialization logic for literal value
        /// </summary>
        protected virtual object DeserializeLiteral(JsonReader reader, Type objectType, JsonSerializer serializer)
        {
            return serializer.Deserialize(reader, objectType);
        }
    }
}
