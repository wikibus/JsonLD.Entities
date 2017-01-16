using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ImpromptuInterface;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NullGuard;
using Vocab;

namespace JsonLD.Entities
{
    /// <summary>
    /// Useful extensions of <see cref="Type"/>
    /// </summary>
    public static class TypeExtension
    {
        private static readonly IDictionary<Type, string> XsdDatatypeMappings;

        static TypeExtension()
        {
            XsdDatatypeMappings = new Dictionary<Type, string>
            {
                [typeof(int)] = Xsd.@int,
                [typeof(uint)] = Xsd.unsignedInt,
                [typeof(long)] = Xsd.@long,
                [typeof(ulong)] = Xsd.unsignedLong,
                [typeof(short)] = Xsd.@short,
                [typeof(ushort)] = Xsd.unsignedShort,
                [typeof(sbyte)] = Xsd.@byte,
                [typeof(byte)] = Xsd.unsignedByte,
                [typeof(DateTime)] = Xsd.dateTime,
                [typeof(DateTimeOffset)] = Xsd.dateTime,
                [typeof(TimeSpan)] = Xsd.duration,
                [typeof(float)] = Xsd.@float,
                [typeof(decimal)] = Xsd.@decimal,
            };
        }

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
            return type.GetCustomAttributes(typeof(SerializeCompactedAttribute), true).Any();
        }

        /// <summary>
        /// Determines whether the type has implicit type in JSON (ie. doesn't require explicit JSON-LD typing)
        /// </summary>
        internal static bool HasImplicitlyTypedJsonType([AllowNull] this Type type)
        {
            return type == null
                || type == typeof(bool)
                || type == typeof(double)
                || type == typeof(string);
        }

        /// <summary>
        /// Gets the URI of an XSD datatype for <paramref name="type" />
        /// </summary>
        internal static string GetXsdType(this Type type)
        {
            string xsdTypeName;
            if (XsdDatatypeMappings.TryGetValue(type, out xsdTypeName) == false)
            {
                throw new ArgumentOutOfRangeException(nameof(type), $"Type {type.Name} doesn't have an XSD equivalent");
            }

            return xsdTypeName;
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
