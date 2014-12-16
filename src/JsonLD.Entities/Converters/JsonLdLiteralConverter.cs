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
        public override void WriteJson(JsonWriter writer, [AllowNull] object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
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
