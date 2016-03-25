using JsonLD.Entities.Context;
using NUnit.Framework;

namespace JsonLD.Entities.Tests.ContextTests
{
    [TestFixture]
    public class VocabContextTests
    {
        [Test]
        public void When_created_should_map_property_using_namespace_prefix()
        {
            // given
            var context = new VocabContext<Issue>("http://example.api/o#");

            // then
            Assert.That(context["title"].ToString(), Is.EqualTo("http://example.api/o#title"));
        }

        public class Issue
        {
            public string Title { get; set; }
        }
    }
}
