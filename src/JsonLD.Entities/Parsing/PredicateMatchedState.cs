namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// State where triple's/quad's predicate has been matched
    /// </summary>
    internal class PredicateMatchedState : NQuadsParserState
    {
        private readonly Node _subjectNode;
        private readonly IriNode _predicateNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="PredicateMatchedState"/> class.
        /// </summary>
        public PredicateMatchedState(NQuadsParserBase parser, int currentLine, Node subjectNode, IriNode predicateNode)
            : base(parser, currentLine)
        {
            _subjectNode = subjectNode;
            _predicateNode = predicateNode;
        }

        /// <summary>
        /// Handles matched object
        /// </summary>
        public override void ObjectMatched(Node objectNode)
        {
            Parser.State = new TripleMatchedState(Parser, CurrentLine, new Triple(_subjectNode, _predicateNode, objectNode));
        }
    }
}
