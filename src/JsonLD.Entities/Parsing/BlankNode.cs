namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// A blank node
    /// </summary>
    internal class BlankNode : Node
    {
        private readonly string _blankNodeId;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlankNode"/> class.
        /// </summary>
        /// <param name="blankNodeId">The blank node identifier.</param>
        public BlankNode(string blankNodeId)
        {
            _blankNodeId = blankNodeId;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public string Identifier
        {
            get { return _blankNodeId; }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return string.Format("_:{0}", _blankNodeId);
        }
    }
}
