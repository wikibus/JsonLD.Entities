using System;
using JsonLD.Core;
using JsonLD.Entities.Parsing;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities
{
    /// <summary>
    /// NQuads parser based on official grammar definition
    /// </summary>
    public class NQuadsParser : IRDFParser
    {
        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        public RDFDataset Parse(JToken input)
        {
            RDFDataset rdfDataset = new RDFDataset();

            var parser = new Parsing.NQuadsParser();
            parser.QuadParsed += (s, a) => AddQuadToDataset(a.Quad.Triple, GetValue(a.Quad.Graph), rdfDataset);
            parser.TripleParsed += (s, a) => AddQuadToDataset(a.Triple, "@default", rdfDataset);

            if (input.Type != JTokenType.String)
            {
                throw new ArgumentException(string.Format("Input must be a string, but got {0}", input.Type), "input");
            }

            parser.Parse((string)input);

            return rdfDataset;
        }

        private void AddQuadToDataset(Triple quad, string graphName, RDFDataset rdfDataset)
        {
            Literal literal = quad.Object as Literal;
            if (literal != null)
            {
                string datatype = null;
                if (literal.Datatype != null)
                {
                    datatype = literal.Datatype.ToString();
                }

                rdfDataset.AddQuad(
                    GetValue(quad.Subject),
                    GetValue(quad.Predicate),
                    GetValue(literal),
                    datatype,
                    literal.LanguageTag,
                    graphName);
            }
            else
            {
                rdfDataset.AddQuad(
                    GetValue(quad.Subject),
                    GetValue(quad.Predicate),
                    GetValue(quad.Object),
                    graphName);
            }
        }

        private string GetValue(Node node)
        {
            var iriNode = node as IriNode;
            if (iriNode != null)
            {
                return iriNode.Iri.ToString();
            }

            var blankNode = node as BlankNode;
            if (blankNode != null)
            {
                return blankNode.Identifier;
            }

            return ((Literal)node).Value;
        }
    }
}
