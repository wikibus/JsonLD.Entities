using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NullGuard;

namespace JsonLD.Entities.Converters
{
    /// <summary>
    /// pending doc
    /// </summary>
    public abstract class JsonLdLiteralConverter : JsonConverter
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
            if (this.ShouldSerializeAsCompactLiteral(value))
            {
                this.WriteJsonLdValue(writer, value, serializer);
                return;
            }

            writer.WriteStartObject();
            writer.WritePropertyName(JsonLdKeywords.Value);

            this.WriteJsonLdValue(writer, value, serializer);

            writer.WritePropertyName(JsonLdKeywords.Type);
            writer.WriteValue(this.GetJsonLdType(value));
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
                return this.ReadJsonLdLiteral(reader, objectType, serializer);
            }

            object value = null;
            while (reader.TokenType != JsonToken.EndObject)
            {
                reader.Read();

                if (reader.TokenType == JsonToken.PropertyName && Equals(reader.Value, JsonLdKeywords.Value))
                {
                    reader.Read();
                    value = this.ReadJsonLdLiteral(reader, objectType, serializer);
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
        /// When overriden in derived classes, it returns a value to determine
        /// whether a compacted or expanded JSON-LD should be produced
        /// </summary>
        protected virtual bool ShouldSerializeAsCompactLiteral(object value)
        {
            return false;
        }

        /// <summary>
        /// When implemented in derived class, returns an RDF data type name for the value
        /// </summary>
        protected virtual string GetJsonLdType(object value)
        {
            throw new NotImplementedException("To serialize as expanded literals it is necessary to override the JsonLdLiteralConverter#GetJsonLdType method");
        }

        /// <summary>
        /// When implemented in derived classes can be used to customize deserialization logic for literal value
        /// </summary>
        protected virtual object ReadJsonLdLiteral(JsonReader reader, Type objectType, JsonSerializer serializer)
        {
            return LiteralSerializer.Deserialize(reader, objectType);
        }

        /// <summary>
        /// When implemented in derived class writes the RDF string representation
        /// of <paramref name="value" />
        /// </summary>
        /// <remarks>
        /// should only use the <see cref="JsonWriter.WriteValue(string)" />
        /// method for writing the RDF value
        /// </remarks>
        protected abstract void WriteJsonLdValue(JsonWriter writer, object value, JsonSerializer serializer);
    }
}
