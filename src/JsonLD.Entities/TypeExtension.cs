using System;
using System.Collections.Generic;
using System.Linq;
using NullGuard;
using Vocab;

namespace JsonLD.Entities
{
    /// <summary>
    /// Useful extension of <see cref="Type"/>
    /// </summary>
    internal static class TypeExtension
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
                [typeof(TimeSpan)] = Xsd.duration,
                [typeof(float)] = Xsd.@float,
                [typeof(decimal)] = Xsd.@decimal,
            };
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
    }
}
