using System;

namespace JsonLD.Entities
{
    /// <summary>
    /// Marker attribute used to inform the <see cref="EntitySerializer"/> that
    /// the annotated class should be compacted after being serialized
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SerializeCompactedAttribute : Attribute
    {
    }
}