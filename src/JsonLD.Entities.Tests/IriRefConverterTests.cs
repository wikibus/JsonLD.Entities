using System.IO;
using JsonLD.Entities.Converters;
using Newtonsoft.Json;
using Xunit;

namespace JsonLD.Entities.Tests
{
    public class IriRefConverterTests
    {
        private readonly IriRefConverter converter = new IriRefConverter();

        [Fact]
        public void Should_serialize_empty_iriref_as_null()
        {
            // given
            var stringWriter = new StringWriter();

            // when
            this.converter.WriteJson(new JsonTextWriter(stringWriter), default(IriRef), new JsonLdSerializer());

            // then
            Assert.Equal("null", stringWriter.ToString());
        }
    }
}
