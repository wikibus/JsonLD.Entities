namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// A state of subject matched
    /// </summary>
    internal class SubjectMatchedState : NQuadsParserState
    {
        private readonly Node _subjectNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectMatchedState"/> class.
        /// </summary>
        public SubjectMatchedState(NQuadsParserBase parser, int currentLine, Node subjectNode)
            : base(parser, currentLine)
        {
            _subjectNode = subjectNode;
        }

        /// <summary>
        /// Handles matched predicate
        /// </summary>
        public override void PredicateMatched(IriNode predicateNode)
        {
            Parser.State = new PredicateMatchedState(Parser, CurrentLine, _subjectNode, predicateNode);
        }
    }
}
