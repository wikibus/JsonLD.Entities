using System;

namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// A literal node
    /// </summary>
    internal class Literal : Node
    {
        private readonly string _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Literal"/> class.
        /// </summary>
        public Literal(string value, Uri datatype, string languageTag)
        {
            _value = value;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return string.Format("\"{0}\"", _value);
        }
    }
}
