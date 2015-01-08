using Eto.Parse;
using Eto.Parse.Parsers;

namespace JsonLD.Entities
{
    public partial class NQuadsParser
    {
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
                                              | new CharRangeTerminal('\xFDF0', '\xFFFD')
                                              | new SurrogatePairRangeTerminal(0x10000, 0xEFFFF)).Named("PN_CHARS_BASE");
        private static readonly Parser PN_CHARS_U = (PN_CHARS_BASE | '_' | ':').Named("PN_CHARS_U");
        private static readonly Parser ECHAR = ('\\' & new CharSetTerminal('t', 'b', 'n', 'r', 'f', '"', '\\')).Named("ECHAR");
        private static readonly Parser PN_CHARS = (PN_CHARS_U | '-' | Terminals.Digit | '\x00B7' | new CharRangeTerminal('\x0300', '\x036F') | new CharRangeTerminal('\x203F', '\x2040')).Named("PN_CHARS");
        private static readonly Parser UCHAR = (("\\u" & HEX & HEX & HEX & HEX) | ("\\U" & HEX & HEX & HEX & HEX & HEX & HEX & HEX & HEX)).Named("UCHAR");
        private static readonly Parser IRIREF_UNRESERVED = Terminals.AnyChar.Except(new CharSetTerminal('<', '>', '"', '{', '}', '|', '^', '`', '\\') | new CharRangeTerminal('\0', '\x20'));
        private static readonly Parser IRIREF = ('<' & -(IRIREF_UNRESERVED | UCHAR) & '>').Named("IRIREF");
        private static readonly Parser STRING_LITERAL_QUOTE = ('"' & -(!new CharSetTerminal('\x22', '\x5C', '\xA', '\xD') | ECHAR | UCHAR) & '"').Named("STRING_LITERAL_QUOTE");
        private static readonly Parser BLANK_NODE_LABEL = ("_:" & -(PN_CHARS_U | Terminals.Digit) & ~(-(PN_CHARS | '.') & PN_CHARS)).Named("BLANK_NODE_LABEL");
        private static readonly Parser LANGTAG = ('@' & +Terminals.Letter & -('-' & +Terminals.Letter)).Named("LANGTAG");
        private static readonly Parser subject = (IRIREF | BLANK_NODE_LABEL).Named("subject");
        private static readonly Parser Ws = -Terminals.WhiteSpace;
        private static readonly Parser predicate = IRIREF.Named("predicate");
        private static readonly Parser literal = (STRING_LITERAL_QUOTE & ~(("^^" & IRIREF) | LANGTAG)).SeparateChildrenBy(Ws).Named("literal");
        private static readonly Parser @object = (IRIREF | BLANK_NODE_LABEL | literal).Named("object");
        private static readonly Parser graphLabel = (IRIREF | BLANK_NODE_LABEL).Named("graphLabel");
        private static readonly Parser statement = (subject & predicate & @object & ~graphLabel & '.').SeparatedBy(Ws).Named("statement");
        // ReSharper restore InconsistentNaming
    }
}