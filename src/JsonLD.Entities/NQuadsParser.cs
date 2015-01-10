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
            if (quad.Object is Literal)
            {
                rdfDataset.AddQuad(
                    GetValue(quad.Subject),
                    GetValue(quad.Predicate),
                    GetValue(quad.Object),
                    ((Literal)quad.Object).Datatype.ToString(),
                    ((Literal)quad.Object).LanguageTag,
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
            if (node is IriNode)
            {
                return ((IriNode)node).Iri.ToString();
            }

            if (node is BlankNode)
            {
                return ((BlankNode)node).Identifier;
            }

            return ((Literal)node).Value;
        }
    }
}
