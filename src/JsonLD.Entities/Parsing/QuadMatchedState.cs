namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// State where quad has been matched
    /// </summary>
    internal class QuadMatchedState : NQuadsParserState
    {
        private readonly Quad _quad;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuadMatchedState"/> class.
        /// </summary>
        public QuadMatchedState(NQuadsParser parser, int currentLine, Quad quad) : base(parser, currentLine)
        {
            _quad = quad;
        }

        /// <summary>
        /// Gets the quad.
        /// </summary>
        public Quad Quad
        {
            get { return _quad; }
        }

        /// <summary>
        /// Handles matched statement
        /// </summary>
        public override void StatementMatched()
        {
            Parser.HandleParsedQuad(Quad);
        }
    }
}
