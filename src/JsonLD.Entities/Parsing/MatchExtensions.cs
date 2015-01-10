using System;
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
        public static Node ToObjectNode(this Match objectMatch)
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
                return new Literal(value, objectMatch["IRIREF", true].ToTriNode().Iri);
            }

            if (objectMatch["LANGTAG", true].Success)
            {
                return new Literal(value, objectMatch["LANGTAG", true].StringValue.Trim('@'));
            }

            return new Literal(value);
        }

        /// <summary>
        /// Converts match to IRI node
        /// </summary>
        public static IriNode ToTriNode(this Match iriMatch)
        {
            var iri = iriMatch.StringValue.Trim('<', '>');
            try
            {
                return new IriNode(iri);
            }
            catch (UriFormatException ex)
            {
                throw new ParsingException(string.Format("Value '{0}' is not a valid absolute IRI", "ARG0"), ex);
            }
        }

        /// <summary>
        /// Converts graph match to IRI or blank node
        /// </summary>
        public static Node ToGraphNode(this Match match)
        {
            if (match["BLANK_NODE_LABEL"].Success)
            {
                return match.ToBlankNode();
            }

            return match.ToTriNode();
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
