namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// A quad
    /// </summary>
    public struct Quad
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Quad"/> struct.
        /// </summary>
        /// <param name="triple">The triple.</param>
        /// <param name="graph">The graph.</param>
        public Quad(Triple triple, IriNode graph)
            : this()
        {
            Triple = triple;
            Graph = graph;
        }

        /// <summary>
        /// Gets the triple.
        /// </summary>
        public Triple Triple { get; private set; }

        /// <summary>
        /// Gets the graph.
        /// </summary>
        public IriNode Graph { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0} {1}", Triple, Graph);
        }
    }
}
