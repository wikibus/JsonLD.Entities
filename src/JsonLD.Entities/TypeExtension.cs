using System;
using System.Linq;

namespace JsonLD.Entities
{
    /// <summary>
    /// Useful extension of <see cref="Type"/>
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
    }
}
