using System;
using System.Diagnostics;
using System.Linq;
using Eto.Parse;
using Eto.Parse.Parsers;

namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// Base parser of NQuads
    /// </summary>
    public abstract class NQuadsParserBase
    {
        public event EventHandler<QuadParsedEventArgs> QuadParsed;
        public event EventHandler<TripleParsedEventArgs> TripleParsed;

        // ReSharper disable InconsistentNaming
        private static readonly Parser HEX = Terminals.HexDigit.Named("HEX");
        private static readonly Parser PN_CHARS_BASE = (Terminals.Letter
                                              | new CharRangeTerminal('\x00C0', '\x00D6')
                                              | new CharRangeTerminal('\x00D8', '\x00F6')
                                              | new CharRangeTerminal('\x00F8', '\x02FF')
                                              | new CharRangeTerminal('\x0370', '\x037D')
                                              | new CharRangeTerminal('\x037F', '\x1FFF')
                                              | new CharRangeTerminal('\x200C', '\x200D')
                                              | new CharRangeTerminal('\x2070', '\x218F')
                                              | new CharRangeTerminal('\x2C00', '\x2FEF')
                                              | new CharRangeTerminal('\x3001', '\xD7FF')
                                              | new CharRangeTerminal('\xF900', '\xFDCF')
                                              | new CharRangeTerminal('\xFDF0', '\xFFFD')).Named("PN_CHARS_BASE");
                                              ////| new SurrogatePairRangeTerminal(0x10000, 0xEFFFF)).Named("PN_CHARS_BASE");
        private static readonly Parser PN_CHARS_U = (PN_CHARS_BASE | '_' | ':').Named("PN_CHARS_U");
        private static readonly Parser ECHAR = ('\\' & new CharSetTerminal('t', 'b', 'n', 'r', 'f', '"', '\\')).Named("ECHAR");
        private static readonly Parser PN_CHARS = (PN_CHARS_U | '-' | Terminals.Digit | '\x00B7' | new CharRangeTerminal('\x0300', '\x036F') | new CharRangeTerminal('\x203F', '\x2040')).Named("PN_CHARS");
        private static readonly Parser UCHAR = (("\\u" & HEX & HEX & HEX & HEX) | ("\\U" & HEX & HEX & HEX & HEX & HEX & HEX & HEX & HEX)).Named("UCHAR");
        private static readonly Parser IRIREF_UNRESERVED = Terminals.AnyChar.Except(new CharSetTerminal('<', '>', '"', '{', '}', '|', '^', '`', '\\') | new CharRangeTerminal('\0', '\x20').Named("test"));
        private static readonly Parser IRIREF = ('<' & -(IRIREF_UNRESERVED | UCHAR) & '>').Named("IRIREF");
        private static readonly Parser STRING_LITERAL_QUOTE = ('"' & -(!new CharSetTerminal('\x22', '\x5C', '\xA', '\xD') | ECHAR | UCHAR) & '"').Named("STRING_LITERAL_QUOTE");
        private static readonly Parser BLANK_NODE_LABEL = ("_:" & -(PN_CHARS_U | Terminals.Digit) & ~(-(PN_CHARS | '.') & PN_CHARS)).Named("BLANK_NODE_LABEL");
        private static readonly Parser LANGTAG = ('@' & +Terminals.Letter & -('-' & +Terminals.Letter)).Named("LANGTAG");
        private static readonly Parser Ws = -Terminals.SingleLineWhiteSpace;
        private static readonly Parser literal = (STRING_LITERAL_QUOTE & ~(("^^" & IRIREF) | LANGTAG)).SeparateChildrenBy(Ws).Named("literal");
        private static readonly Parser comment = ~(Ws & '#' & -Terminals.AnyChar.Except(Terminals.Eol)).SeparatedBy(Ws);
        private static readonly Parser eol = (comment & Terminals.Eol) | -Terminals.Eol;
        // ReSharper restore InconsistentNaming

        private readonly Grammar _grammar;
        private NQuadsParserState _state;

        protected NQuadsParserBase()
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

            _grammar = new Grammar(~eol & (-statement).SeparatedBy(-eol) & -eol);
            _state = new LineStartState(this, 1);
        }

        /// <summary>
        /// Gets the state.
        /// </summary>
        internal NQuadsParserState State
        {
            get { return _state; }
            set { _state = value; }
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

        protected virtual void OnQuadParsed(QuadParsedEventArgs e)
        {
            var handler = QuadParsed;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnTripleParsed(TripleParsedEventArgs e)
        {
            var handler = TripleParsed;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Parses the specified RDF.
        /// </summary>
        protected void Parse(string nquads)
        {
            var result = _grammar.Match(nquads);

            if (result.Success == false)
            {
                var message = string.Format("Unrecognized token at position {0}", result.ErrorIndex);
                Debug.WriteLine(message);
                throw new ParsingException(message);
            }
        }

        private void OnSubjectMatched(Match subjectMatch)
        {
            Debug.WriteLine("Parsed subject {0}", subjectMatch);

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
            Debug.WriteLine("Parsed predicate {0}", match);

            _state.PredicateMatched(match.ToTriNode());
        }

        private void OnObjectMatched(Match match)
        {
            Debug.WriteLine("Parsed object {0}", match);

            _state.ObjectMatched(match.ToObjectNode());
        }

        private void OnGraphLabelMatched(Match match)
        {
            Debug.WriteLine("Parsed graph {0}", match);

            _state.GraphMatched(match.ToGraphNode());
        }

        private void HandleStatement()
        {
            _state.StatementMatched();

        }
    }
}
