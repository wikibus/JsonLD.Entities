using System;

namespace JsonLD.Entities
{
    /// <summary>
    /// Allow annotating a model's RDF type
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class ClassAttribute : Attribute
    {
        private readonly Uri _classUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassAttribute"/> class.
        /// </summary>
        /// <param name="classUri">The class URI.</param>
        public ClassAttribute(string classUri)
        {
            _classUri = new Uri(classUri);
        }

        /// <summary>
        /// Gets the RDF class URI.
        /// </summary>
        public Uri ClassUri
        {
            get { return _classUri; }
        }
    }
}
