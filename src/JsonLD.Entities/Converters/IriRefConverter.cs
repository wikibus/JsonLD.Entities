using System;
using Newtonsoft.Json;
using NullGuard;

namespace JsonLD.Entities.Converters
{
    /// <summary>
    /// Converter of <see cref="IriRef"/>
    /// </summary>
    public class IriRefConverter : JsonConverter
    {
        /// <summary>
        /// Writes the JSON representing the <see cref="IriRef"/> <paramref name="value"/>.
        /// </summary>
        public override void WriteJson(JsonWriter writer, [AllowNull] object value, JsonSerializer serializer)
        {
            var iriRef = (IriRef)value;
            if (iriRef == default(IriRef))
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.WritePropertyName(JsonLdKeywords.Id);
                writer.WriteValue(iriRef.Value.ToString());
                writer.WriteEndObject();
            }
        }

        /// <summary>
        /// Reads the JSON into the a <see cref="IriRef"/> object.
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                return new IriRef(reader.Value.ToString());
            }

            if (reader.TokenType == JsonToken.StartObject)
            {
                string iriRef = null;

                while (reader.Read() && reader.TokenType != JsonToken.EndObject)
                {
                    if (reader.TokenType != JsonToken.PropertyName || (string)reader.Value != JsonLdKeywords.Id)
                    {
                        continue;
                    }

                    reader.Read();
                    iriRef = (string)reader.Value;
                }

                return iriRef != null ? new IriRef(iriRef) : default(IriRef);
            }

            throw new InvalidOperationException(string.Format("Cannot deserialize token type {0} as IriRef", reader.TokenType));
        }

        /// <summary>
        /// True if <paramref name="objectType"/> is an <see cref="IriRef"/>
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IriRef);
        }
    }
}