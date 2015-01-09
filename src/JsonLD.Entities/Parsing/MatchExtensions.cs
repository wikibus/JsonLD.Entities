using Eto.Parse;

namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// Helpers methods for turning <see cref="Match"/> into <see cref="Node"/>
    /// </summary>
    internal static class MatchExtensions
    {
        /// <summary>
        /// Converts object match to a node
        /// </summary>
        public static Node GetObjectNode(this Match objectMatch)
        {
            if (objectMatch["IRIREF"].Success)
            {
                return objectMatch.ToTriNode();
            }

            if (objectMatch["BLANK_NODE_LABEL", true].Success)
            {
                return objectMatch.ToBlankNode();
            }

            var value = objectMatch["STRING_LITERAL_QUOTE", true].StringValue.Trim('"');

            if (objectMatch["IRIREF", true].Success)
            {
                return new Literal(value, objectMatch["IRIREF", true].ToTriNode().Iri, null);
            }

            if (objectMatch["LANGTAG", true].Success)
            {
                return new Literal(value, null, objectMatch["LANGTAG"].StringValue.Trim('@'));
            }

            return new Literal(value, null, null);
        }

        /// <summary>
        /// Converts match to IRI node
        /// </summary>
        public static IriNode ToTriNode(this Match iriMatch)
        {
            return new IriNode(iriMatch.StringValue.Trim('<', '>'));
        }

        /// <summary>
        /// Converts match to blank node
        /// </summary>
        public static BlankNode ToBlankNode(this Match blankNodeMatch)
        {
            return new BlankNode(blankNodeMatch.StringValue.Replace("_:", string.Empty));
        }
    }
}
