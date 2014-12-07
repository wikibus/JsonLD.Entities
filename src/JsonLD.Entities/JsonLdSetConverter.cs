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
    public class JsonLdSetConverter<T> : JsonConverter
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
            var result = new HashSet<T>();

            if (reader.TokenType == JsonToken.StartArray)
            {
                reader.Read();

                while (reader.TokenType != JsonToken.EndArray)
                {
                    var item = serializer.Deserialize<T>(reader);
                    result.Add(item);
                    reader.Read();
                }
            }
            else
            {
                // JSON object was not an array,
                // so deserialize the object and wrap it in a List.
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
