namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// State where triple has been matched
    /// </summary>
    internal class TripleMatchedState : NQuadsParserState
    {
        private readonly Triple _triple;

        /// <summary>
        /// Initializes a new instance of the <see cref="TripleMatchedState"/> class.
        /// </summary>
        public TripleMatchedState(NQuadsParserBase parser, int currentLine, Triple triple) : base(parser, currentLine)
        {
            _triple = triple;
        }

        /// <summary>
        /// Gets the triple.
        /// </summary>
        public Triple Triple
        {
            get { return _triple; }
        }

        /// <summary>
        /// Handles matched graph
        /// </summary>
        public override void GraphMatched(IriNode graphNode)
        {
            Parser.State = new QuadMatchedState(Parser, CurrentLine, new Quad(Triple, graphNode));
        }

        /// <summary>
        /// Handles matched statement
        /// </summary>
        public override void StatementMatched()
        {
            Parser.HandleParsedTriple(Triple);
            AdvanceToNextLine();
        }
    }
}
