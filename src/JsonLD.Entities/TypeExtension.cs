using System;
using System.Linq;
using System.Reflection;
using ImpromptuInterface;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonLD.Entities
{
    /// <summary>
    /// Useful extensions of <see cref="Type"/>
    /// </summary>
    internal static class TypeExtension
    {
        /// <summary>
        /// Determines whether the <paramref name="type"/> should be compacted after serialization.
        /// </summary>
        internal static bool IsMarkedForCompaction(this Type type)
        {
            return type.GetCustomAttributes(typeof(SerializeCompactedAttribute), true).Any();
        }

        /// <summary>
        /// Gets the class identifier for an entity type.
        /// </summary>
        internal static Uri GetTypeIdentifier(this Type type)
        {
            var typesProperty = type.GetProperty("Type", BindingFlags.Static | BindingFlags.NonPublic) ??
                                type.GetProperty("Types", BindingFlags.Static | BindingFlags.NonPublic) ??
                                type.GetAnnotatedTypeProperty();

            if (typesProperty == null)
            {
                throw new InvalidOperationException($"Type {type} does not statically declare @type");
            }

            var getter = typesProperty.GetGetMethod(true);
            dynamic typeValue = getter.Invoke(null, null);

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

        private static PropertyInfo GetAnnotatedTypeProperty(this Type type)
        {
            return (from prop in type.GetProperties(BindingFlags.Static)
                    let jsonProperty = prop.GetCustomAttributes(typeof(JsonPropertyAttribute), false).SingleOrDefault() as JsonPropertyAttribute
                    where jsonProperty != null
                    where jsonProperty.PropertyName == JsonLdKeywords.Type
                    select prop).FirstOrDefault();
        }
    }
}
