using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NullGuard;

namespace JsonLD.Entities.Converters
{
    /// <summary>
    /// Converter for JSON-LD @list
    /// </summary>
    /// <typeparam name="T">collection element type</typeparam>
    public class JsonLdListConverter<T> : JsonLdCollectionConverter<T>
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
            if (reader.TokenType == JsonToken.StartObject)
            {
                reader.Read();
                while (Equals("@list", reader.Value) == false)
                {
                    reader.Read();
                }

                reader.Read();
                var actualCollection = serializer.Deserialize(reader, objectType);
                reader.Read();
                return actualCollection;
            }

            return base.ReadJson(reader, objectType, existingValue, serializer);
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            return typeof(IList<T>).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Wraps elements in <see cref="List{T}"/>
        /// </summary>
        protected override object CreateReturnedContainer(IEnumerable<T> elements)
        {
            return new List<T>(elements);
        }
    }
}
