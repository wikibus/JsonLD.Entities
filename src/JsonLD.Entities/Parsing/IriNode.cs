using System;

namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// A IRI node
    /// </summary>
    public class IriNode : Node
    {
        private readonly Uri _iri;

        /// <summary>
        /// Initializes a new instance of the <see cref="IriNode"/> class.
        /// </summary>
        public IriNode(string iri)
        {
            _iri = new Uri(iri);
        }

        /// <summary>
        /// Gets the IRI.
        /// </summary>
        public Uri Iri
        {
            get { return _iri; }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return string.Format("<{0}>", Iri);
        }
    }
}
