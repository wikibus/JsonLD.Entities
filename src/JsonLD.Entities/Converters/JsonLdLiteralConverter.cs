using System;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NullGuard;

namespace JsonLD.Entities.Converters
{
    /// <summary>
    /// pending doc
    /// </summary>
    public class JsonLdLiteralConverter : JsonConverter
    {
        private static readonly JsonLdSerializer LiteralSerializer;

        /// <summary>
        /// Initializes static members of the <see cref="JsonLdLiteralConverter"/> class.
        /// </summary>
        static JsonLdLiteralConverter()
        {
            LiteralSerializer = new JsonLdSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        public override void WriteJson(JsonWriter writer, [AllowNull] object value, JsonSerializer serializer)
        {
            var type = value?.GetType();
            if (type?.HasImplicitlyTypedJsonType() == true)
            {
                writer.WriteValue(value);
                return;
            }

            writer.WriteStartObject();
            writer.WritePropertyName(JsonLdKeywords.Value);

            if (value is TimeSpan)
            {
                writer.WriteValue(XmlConvert.ToString((TimeSpan)value));
            }
            else
            {
                var valueString = JsonConvert.ToString(value);
                writer.WriteValue(valueString.Trim('"')); // for the weirdest of reasons, DateTime is double-quoted
            }

            writer.WritePropertyName(JsonLdKeywords.Type);
            writer.WriteValue(type.GetXsdType());
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        public sealed override object ReadJson(
            JsonReader reader,
            Type objectType,
            [AllowNull] object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.StartObject)
            {
                return this.DeserializeLiteral(reader, objectType, serializer);
            }

            object value = null;
            while (reader.TokenType != JsonToken.EndObject)
            {
                reader.Read();

                if (reader.TokenType == JsonToken.PropertyName && Equals(reader.Value, JsonLdKeywords.Value))
                {
                    reader.Read();
                    value = this.DeserializeLiteral(reader, objectType, serializer);
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
            return LiteralSerializer.Deserialize(reader, objectType);
        }
    }
}
