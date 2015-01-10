namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// Abstract state of <see cref="NQuadsParser"/>
    /// </summary>
    internal abstract class NQuadsParserState
    {
        private readonly NQuadsParser _parser;
        private readonly int _currentLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="NQuadsParserState"/> class.
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <param name="currentLine">The current line.</param>
        protected NQuadsParserState(NQuadsParser parser, int currentLine)
        {
            _parser = parser;
            _currentLine = currentLine;
        }

        /// <summary>
        /// Gets the current line.
        /// </summary>
        /// <value>
        /// The current line.
        /// </value>
        public int CurrentLine
        {
            get { return _currentLine; }
        }

        /// <summary>
        /// Gets the parser.
        /// </summary>
        /// <value>
        /// The parser.
        /// </value>
        protected NQuadsParser Parser
        {
            get { return _parser; }
        }

        /// <summary>
        /// Handles matched subject
        /// </summary>
        public virtual void SubjectMatched(Node subjectNode)
        {
            throw new System.NotImplementedException(GetType().Name);
        }

        /// <summary>
        /// Handles matched graph
        /// </summary>
        public virtual void GraphMatched(Node graphNode)
        {
            throw new System.NotImplementedException(GetType().Name);
        }

        /// <summary>
        /// Handles matched predicate
        /// </summary>
        public virtual void PredicateMatched(IriNode predicateNode)
        {
            throw new System.NotImplementedException(GetType().Name);
        }

        /// <summary>
        /// Handles matched object
        /// </summary>
        public virtual void ObjectMatched(Node objectNode)
        {
            throw new System.NotImplementedException(GetType().Name);
        }

        /// <summary>
        /// Handles matched statement
        /// </summary>
        public virtual void StatementMatched()
        {
            throw new System.NotImplementedException(GetType().Name);
        }

        /// <summary>
        /// Advances parser state to next line.
        /// </summary>
        public void AdvanceToNextLine()
        {
            Parser.State = new LineStartState(Parser, CurrentLine + 1);
        }
    }
}
