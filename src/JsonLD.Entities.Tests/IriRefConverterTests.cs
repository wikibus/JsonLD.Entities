using System.IO;
using JsonLD.Entities.Converters;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JsonLD.Entities.Tests
{
    [TestFixture]
    public class IriRefConverterTests
    {
        private readonly IriRefConverter converter = new IriRefConverter();

        [Test]
        public void Should_serialize_empty_iriref_as_null()
        {
            // given
            var stringWriter = new StringWriter();

            // when
            this.converter.WriteJson(new JsonTextWriter(stringWriter), default(IriRef), new JsonLdSerializer());

            // then
            Assert.That(stringWriter.ToString(), Is.EqualTo("null"));
        }
    }
}
