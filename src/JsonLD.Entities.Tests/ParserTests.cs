using System.Collections.Generic;
using JsonLD.Core;
using NUnit.Framework;
using Resourcer;

namespace JsonLD.Entities.Tests
{
    [TestFixture]
    public class ParserTests
    {
        private NQuadsParser _parser;

        [SetUp]
        public void Setup()
        {
            _parser = new NQuadsParser();
        }

        [Test]
        public void ParseTest()
        {
            // given
            string quads = Resource.AsString("NQuads.Literals.nq");

            // when
            var dataset = _parser.Parse(quads);
            var defaultGraph = (IList<RDFDataset.Quad>)dataset["@default"];

            // then
            Assert.That(defaultGraph, Has.Count.EqualTo(8));
        }
    }
}
