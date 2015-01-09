namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// A triple
    /// </summary>
    public struct Triple
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Triple"/> struct.
        /// </summary>
        public Triple(Node subject, IriNode predicate, Node obj)
            : this()
        {
            Subject = subject;
            Predicate = predicate;
            Object = obj;
        }

        /// <summary>
        /// Gets the subject.
        /// </summary>
        public Node Subject { get; private set; }

        /// <summary>
        /// Gets the predicate.
        /// </summary>
        public IriNode Predicate { get; private set; }

        /// <summary>
        /// Gets the object.
        /// </summary>
        public Node Object { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Subject, Predicate, Object);
        }
    }
}
