using System;
using System.Collections.Generic;
using System.IO;
using JsonLD.Core;
using JsonLD.Entities.Parsing;
using JsonLD.Impl;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonLD.Entities.Tests
{
    [TestFixture]
    public class ParserTests
    {
        private const string BasePath = @"ParsingTests\NQuads\";
        private const string ManifestPath = BasePath + "manifest.ttl";
        private static readonly JObject ManifestFrame = JObject.Parse(@"
{
    '@context': {
        'mf': 'http://www.w3.org/2001/sw/DataAccess/tests/test-manifest#',
        'rdfs': 'http://www.w3.org/2000/01/rdf-schema#',
        'rdft': 'http://www.w3.org/ns/rdftest#',
        'mf:entries': { '@container': '@list'},
        'mf:action': { '@type': '@id'}
    },
    '@type': 'mf:Manifest'
}");

        private NQuadsParserTestable _parser;

        [SetUp]
        public void Setup()
        {
            _parser = new NQuadsParserTestable();
        }

        [TestCaseSource("GetTestCases")]
        public void ParseTest(string path)
        {
            // given
            string quads = File.ReadAllText(BasePath + path);

            // when
            _parser.Parse(quads);
        }

        private static IEnumerable<TestCaseData> GetTestCases()
        {
            var manifest = JsonLdProcessor.FromRDF(File.ReadAllText(ManifestPath), new TurtleRDFParser());
            var framed = JsonLdProcessor.Frame(manifest, ManifestFrame, new JsonLdOptions());

            foreach (var testCase in framed["@graph"][0]["mf:entries"])
            {
                var testCaseData = new TestCaseData((string)testCase["mf:action"])
                    .SetName(((string)testCase["mf:name"]).Trim('"'))
                    .SetDescription(((string)testCase["rdfs:comment"]).Trim('"'));

                if ((string)testCase["@type"] == "rdft:TestNQuadsNegativeSyntax")
                {
                    testCaseData.Throws(typeof(ParsingException));
                }

                yield return testCaseData;
            }
        }

        public class NQuadsParserTestable
        {
            public void Parse(string quads)
            {
                Parsing.NQuadsParser parser = new Parsing.NQuadsParser();

                parser.QuadParsed += HandleParsedQuad;
                parser.TripleParsed += HandleParsedTriple;

                parser.Parse(quads);
            }

            private void HandleParsedQuad(object sender, QuadParsedEventArgs args)
            {
                Console.WriteLine("Parsed quad '{0}' on line {1}", args.Quad, args.Line);
            }

            private void HandleParsedTriple(object sender, TripleParsedEventArgs args)
            {
                Console.WriteLine("Parsed triple '{1}' on line {0}", args.Line, args.Triple);
            }
        }
    }
}
