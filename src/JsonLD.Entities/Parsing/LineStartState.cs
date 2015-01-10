using System;

namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// A new line state
    /// </summary>
    internal class LineStartState : NQuadsParserState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineStartState"/> class.
        /// </summary>
        /// <param name="parser">The parser.</param>
        /// <param name="currentLine">The current line.</param>
        public LineStartState(NQuadsParser parser, int currentLine)
            : base(parser, currentLine)
        {
        }

        /// <summary>
        /// Handles matched subject
        /// </summary>
        public override void SubjectMatched(Node subjectNode)
        {
            Parser.State = new SubjectMatchedState(Parser, CurrentLine, subjectNode);
        }
    }
}
