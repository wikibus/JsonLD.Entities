using System;

namespace JsonLD.Entities
{
    /// <summary>
    /// Allow annotating a model's RDF type
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class ClassAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassAttribute"/> class.
        /// </summary>
        /// <param name="classUriOrPrefixedName">The class URI.</param>
        public ClassAttribute(string classUriOrPrefixedName)
        {
            Class = classUriOrPrefixedName;
        }

        /// <summary>
        /// Gets the RDF class URI.
        /// </summary>
        public string Class { get; private set; }
    }
}
