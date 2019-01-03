using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonLD.Entities
{
    /// <summary>
    /// Useful extensions of <see cref="Type"/>
    /// </summary>
    public static class TypeExtension
    {
        /// <summary>
        /// Gets the class identifier for an entity type.
        /// </summary>
        public static Uri GetTypeIdentifier(this Type type)
        {
            var typesProperty = type.GetProperty("Type", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public) ??
                                type.GetProperty("Types", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public) ??
                                type.GetAnnotatedTypeProperty();

            if (typesProperty == null)
            {
                throw new InvalidOperationException($"Type {type} does not statically declare @type");
            }

            var getter = typesProperty.GetGetMethod(true);
            dynamic typeValue = getter.Invoke(null, null);

            if (typeValue is IEnumerable && Enumerable.Count(typeValue) == 1)
            {
                typeValue = Enumerable.Single(typeValue);
            }

            if (typeValue is Uri)
            {
                return typeValue;
            }

            if (typeValue is string)
            {
                return new Uri(typeValue);
            }

            throw new InvalidOperationException("Cannot convert value to Uri");
        }

        /// <summary>
        /// Determines whether the <paramref name="type"/> should be compacted after serialization.
        /// </summary>
        internal static bool IsMarkedForCompaction(this Type type)
        {
            return type.GetTypeInfo().GetCustomAttributes(typeof(SerializeCompactedAttribute), true).Any();
        }

        private static PropertyInfo GetAnnotatedTypeProperty(this Type type)
        {
            return (from prop in type.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                    let jsonProperty = prop.GetCustomAttributes(typeof(JsonPropertyAttribute), false).SingleOrDefault() as JsonPropertyAttribute
                    where jsonProperty != null
                    where jsonProperty.PropertyName == JsonLdKeywords.Type
                    select prop).FirstOrDefault();
        }
    }
}
