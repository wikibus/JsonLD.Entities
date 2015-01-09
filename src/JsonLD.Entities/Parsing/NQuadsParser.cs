using System;
using JsonLD.Core;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// NQuads parser based on official grammar definition
    /// </summary>
    public class NQuadsParser : NQuadsParserBase, IRDFParser
    {
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

            base.Parse((string)input);

            return new RDFDataset();
        }
    }
}
