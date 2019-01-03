using JsonLD.Entities.Context;
using Xunit;

namespace JsonLD.Entities.Tests.ContextTests
{
    public class VocabContextTests
    {
        [Fact]
        public void When_created_should_map_property_using_namespace_prefix()
        {
            // given
            var context = new VocabContext<Issue>("http://example.api/o#");

            // then
            Assert.Equal("http://example.api/o#title", context["title"].ToString());
        }

        public class Issue
        {
            public string Title { get; set; }
        }
    }
}
