using System;
using Eto.Parse;

namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// Base parser of NQuads
    /// </summary>
    public sealed partial class NQuadsParser
    {
        private readonly Grammar _grammar;
        private NQuadsParserState _state;

        /// <summary>
        /// Initializes a new instance of the <see cref="NQuadsParser"/> class.
        /// </summary>
        public NQuadsParser()
        {
            var subject = (IRIREF | BLANK_NODE_LABEL).Named("subject");
            subject.Matched += OnSubjectMatched;

            var predicate = IRIREF.Named("predicate");
            predicate.Matched += OnPredicateMatched;

            var @object = (IRIREF | BLANK_NODE_LABEL | literal).Named("object");
            @object.Matched += OnObjectMatched;

            var graphLabel = (IRIREF | BLANK_NODE_LABEL).Named("graphLabel");
            graphLabel.Matched += OnGraphLabelMatched;

            var statement = ((subject & predicate & @object & ~graphLabel & '.').SeparatedBy(Ws) & ~comment).Named("statement");
            statement.Matched += match => HandleStatement();

            var line = (((Ws & statement) | (Ws & ~comment)) & Terminals.Eol).Named("documentLine");
            line.Matched += HandleNewLine;

            _grammar = new Grammar(-line);
            _state = new LineStartState(this, 1);
        }

        /// <summary>
        /// Occurs when a quad has been parsed.
        /// </summary>
        public event EventHandler<QuadParsedEventArgs> QuadParsed;

        /// <summary>
        /// Occurs when a triple has been parsed.
        /// </summary>
        public event EventHandler<TripleParsedEventArgs> TripleParsed;

        /// <summary>
        /// Sets the state.
        /// </summary>
        internal NQuadsParserState State
        {
            set { _state = value; }
        }

        /// <summary>
        /// Parses the specified RDF.
        /// </summary>
        public void Parse(string nquads)
        {
            _grammar.AllowPartialMatch = true;
            var result = _grammar.Match(nquads);

            if (result.Success == false)
            {
                throw new ParsingException(string.Format("Unrecognized token at position {0}", result.ErrorIndex));
            }
        }

        /// <summary>
        /// Handles the parsed quad.
        /// </summary>
        internal void HandleParsedQuad(Quad quad)
        {
            OnQuadParsed(new QuadParsedEventArgs(quad, _state.CurrentLine));
        }

        /// <summary>
        /// Handles the parsed triple.
        /// </summary>
        internal void HandleParsedTriple(Triple triple)
        {
            OnTripleParsed(new TripleParsedEventArgs(triple, _state.CurrentLine));
        }

        private void OnQuadParsed(QuadParsedEventArgs e)
        {
            var handler = QuadParsed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnTripleParsed(TripleParsedEventArgs e)
        {
            var handler = TripleParsed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnSubjectMatched(Match subjectMatch)
        {
            if (subjectMatch[IRIREF.Name].Success)
            {
                _state.SubjectMatched(subjectMatch.ToTriNode());
            }
            else
            {
                _state.SubjectMatched(subjectMatch.ToBlankNode());
            }
        }

        private void OnPredicateMatched(Match match)
        {
            _state.PredicateMatched(match.ToTriNode());
        }

        private void OnObjectMatched(Match match)
        {
            _state.ObjectMatched(match.ToObjectNode());
        }

        private void OnGraphLabelMatched(Match match)
        {
            _state.GraphMatched(match.ToGraphNode());
        }

        private void HandleStatement()
        {
            _state.StatementMatched();
        }

        private void HandleNewLine(Match match)
        {
            _state.AdvanceToNextLine();
        }
    }
}
