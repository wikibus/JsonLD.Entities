using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JsonLD.Entities.Converters
{
    /// <summary>
    /// Converter for JSON-LD @sets
    /// </summary>
    /// <typeparam name="T">collection element type</typeparam>
    public class JsonLdArrayConverter<T> : JsonLdCollectionConverter<T>
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            return typeof(T[]).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Creates an array of <typeparamref name="T"/>
        /// </summary>
        protected override object CreateReturnedContainer(IEnumerable<T> elements)
        {
            return elements.ToArray();
        }
    }
}
