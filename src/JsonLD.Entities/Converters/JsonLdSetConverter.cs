using System;
using System.Collections.Generic;
using System.Reflection;

namespace JsonLD.Entities.Converters
{
    /// <summary>
    /// Converter for JSON-LD @sets
    /// </summary>
    /// <typeparam name="T">collection element type</typeparam>
    public class JsonLdSetConverter<T> : JsonLdCollectionConverter<T>
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            return typeof(ISet<T>).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Wraps the elements in <see cref="HashSet{T}"/>
        /// </summary>
        protected override object CreateReturnedContainer(IEnumerable<T> elements)
        {
            return new HashSet<T>(elements);
        }
    }
}
