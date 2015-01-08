using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eto.Parse;
using JsonLD.Core;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities
{
    /// <summary>
    /// NQuads parser based on official grammar definition
    /// </summary>
    public partial class NQuadsParser : IRDFParser
    {
        private static readonly Grammar Grammar;

        static NQuadsParser()
        {
            Grammar = new Grammar(~EOL & (-statement).SeparatedBy(-EOL) & -EOL);
        }

        /// <summary>
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        public RDFDataset Parse(JToken input)
        {
            if (input.Type != JTokenType.String)
            {
                throw new ArgumentException(string.Format("Input must be a string, but got {0}", input.Type), "input");
            }

            var parsingResult = Grammar.Match((string)input);
            var result = new RDFDataset();

            if (parsingResult.Success == false)
            {
                Debug.WriteLine(parsingResult.ErrorMessage);
                throw new Exception(string.Format("Could not parse NQuads at {0}", parsingResult.ErrorIndex));
            }

            foreach (var match in parsingResult.Matches)
            {
                var subj = GetSubjectNode(match["subject"]);
                var pred = new RDFDataset.IRI(GetIri(match["predicate"]));
                var obj = GetObjectNode(match["object"]);
                var graphName = "@default";

                if (match["graphLabel"].Success)
                {
                    graphName = GetIri(match["graphLabel"]);
                }

                var quad = new RDFDataset.Quad(subj, pred, obj, graphName);
                var graph = GetGraph(result, graphName);
                if (!graph.Contains(quad))
                {
                    graph.Add(quad);
                }
            }

            return result;
        }

        private static IList<RDFDataset.Quad> GetGraph(RDFDataset dataset, string graph)
        {
            if (dataset.ContainsKey(graph) == false)
            {
                dataset.Add(graph, new List<RDFDataset.Quad>());
            }

            return (IList<RDFDataset.Quad>)dataset[graph];
        }

        private RDFDataset.Node GetObjectNode(Match objectMatch)
        {
            if (objectMatch["IRIREF"].Success)
            {
                return new RDFDataset.IRI(GetIri(objectMatch));
            }

            if (objectMatch["BLANK_NODE_LABEL", true].Success)
            {
                return new RDFDataset.BlankNode(GetBlankNodeId(objectMatch));
            }

            var value = objectMatch["STRING_LITERAL_QUOTE", true].StringValue.Trim('"');

            if (objectMatch["IRIREF", true].Success)
            {
                return new RDFDataset.Literal(value, GetIri(objectMatch["IRIREF", true]), null);
            }

            if (objectMatch["LANGTAG", true].Success)
            {
                return new RDFDataset.Literal(value, null, objectMatch["LANGTAG"].StringValue.Trim('@'));
            }

            return new RDFDataset.Literal(value, null, null);
        }

        private RDFDataset.Node GetSubjectNode(Match subjectMatch)
        {
            if (subjectMatch["IRIREF"].Success)
            {
                return new RDFDataset.IRI(GetIri(subjectMatch));
            }

            return new RDFDataset.BlankNode(GetBlankNodeId(subjectMatch));
        }

        private string GetIri(Match iriMatch)
        {
            return iriMatch.StringValue.Trim('<', '>');
        }

        private string GetBlankNodeId(Match blankNodeMatch)
        {
            return blankNodeMatch.StringValue;
        }
    }
}
