using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using NullGuard;

namespace JsonLD.Entities
{
    /// <summary>
    /// Converter for JSON-LD @sets
    /// </summary>
    /// <typeparam name="T">collection element type</typeparam>
    public class JsonLdListConverter<T> : JsonConverter
    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        public override void WriteJson(JsonWriter writer, [AllowNull] object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
        {
            var result = new List<T>();

            if (reader.TokenType == JsonToken.StartArray)
            {
                reader.Read();

                while (reader.TokenType != JsonToken.EndArray)
                {
                    result.Add(serializer.Deserialize<T>(reader));
                    reader.Read();
                }
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                reader.Read();
                while (Equals("@list", reader.Value) == false)
                {
                    reader.Read();
                }

                reader.Read();
                var result1 = serializer.Deserialize(reader, objectType);
                reader.Read();
                return result1;
            }
            else
            {
                var resultObject = serializer.Deserialize<T>(reader);
                result.Add(resultObject);
            }

            return result;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            return typeof(IEnumerable).IsAssignableFrom(objectType);
        }
    }
}
