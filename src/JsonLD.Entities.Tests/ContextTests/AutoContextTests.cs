using System;
using JsonLD.Entities.Context;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace JsonLD.Entities.Tests.ContextTests
{
    public class AutoContextTests
    {
        [Fact]
        public void When_created_should_include_all_properties()
        {
            // given
            var context = new AutoContext<Issue>(new Uri("http://example.api/o#Issue"));

            // then
            Assert.Equal(6, context.Count);
        }

        [Fact]
        public void When_created_should_not_include_reserved_keywords()
        {
            // given
            var context = new AutoContext<Issue>(new Uri("http://example.api/o#Issue"));

            // then
            Assert.Null(context[JsonLdKeywords.Id]);
            Assert.Null(context[JsonLdKeywords.Type]);
            Assert.Null(context[JsonLdKeywords.Context]);
        }

        [Fact]
        public void When_created_should_respect_Newtonsoft_property_attribute()
        {
            // given
            var context = new AutoContext<Issue>(new Uri("http://example.api/o#Issue"));

            // then
            Assert.NotNull(context["titel"]);
        }

        [Fact]
        public void When_created_should_extend_given_context_object()
        {
            // given
            var manualContext = JObject.Parse("{ 'titel': 'dcterms:title' }");

            // when
            var context = new AutoContext<Issue>(manualContext, new Uri("http://example.api/o#Issue"));

            // then
            Assert.Equal("dcterms:title", context["titel"].ToString());
        }

        [Fact]
        public void When_modified_should_allow_changing_property_definition()
        {
            // given
            var context = new AutoContext<Issue>(new Uri("http://example.api/o#Issue"));
            var expectedMapping = JObject.Parse(@"{
  '@id': 'http://example.api/o#Issue/projectId',
  '@type': '@id'
}");

            // when
            context.Property(i => i.ProjectId, propName => propName.Type().Id());

            // then
            Assert.True(JToken.DeepEquals(context["projectId"], expectedMapping));
        }

        [Fact]
        public void When_modified_should_allow_remapping_expanded_property_definition()
        {
            // given
            var contextBefore = JObject.Parse(@"{
  'projectId': {
    '@id': 'http://example.api/o#Issue/projectId',
    '@type': '@vocab'
  }
}");
            var context = new AutoContext<Issue>(contextBefore, new Uri("http://example.api/o#Issue"));
            var contextAfter = JObject.Parse(@"
{
  '@id': 'http://example.api/o#Issue/projectId',
  '@type': '@id'
}");

            // when
            context.Property(i => i.ProjectId, propName => propName.Type().Id());

            // then
            Assert.True(JToken.DeepEquals(context["projectId"], contextAfter));
        }

        [Theory]
        [InlineData("http://example.org/o#Issue", "http://example.org/o#Issue/projectId")]
        [InlineData("http://example.org/o/Issue", "http://example.org/o/Issue#projectId")]
        public void Should_concatenate_with_separator_depending_on_class_id(string issueClassStr, string expectedPropertyId)
        {
            // when
            var context = new AutoContext<Issue>(new Uri(issueClassStr));

            // then
            Assert.Equal(expectedPropertyId, context["projectId"].ToString());
        }

        [Fact]
        public void Should_use_value_set_to_JsonProperty_attribute_for_concatentation()
        {
            // given
            var context = new AutoContext<User>(new Uri("http://example.org/ontolgy/User"));

            // then
            Assert.Equal("http://example.org/ontolgy/User#with_attribute", context["with_attribute"].ToString());
        }

        [Fact]
        public void Should_preserve_existing_context_when_using_declared_class_type()
        {
            // given
            var existing = JObject.Parse("{ 'titel': 'dcterms:title' }");
            var context = new AutoContext<Issue>(existing);

            // then
            Assert.Equal("dcterms:title", context["titel"].ToString());
        }

        private class Issue
        {
            private const string IssueType = "http://example.api/o#Issue";

            public string Id { get; set; }

            [JsonProperty("titel")]
            public string Title { get; set; }

            public string Content { get; set; }

            public int LikesCount { get; private set; }

            public User Submitter { get; set; }

            public bool IsResolved { get; set; }

            public IriRef ProjectId { get; set; }

            private static JToken Context { get; set; }

            [JsonProperty]
            private static string Type
            {
                get { return IssueType; }
            }

            public string Method()
            {
                return null;
            }
        }

        private class User
        {
            public static Uri Type { get; set; }

            public static JObject Context { get; set; }

            public string Id { get; set; }

            [JsonProperty("firstName")]
            public string Name { get; set; }

            public string LastName { get; set; }

            [JsonProperty("with_attribute")]
            public string NotInContextWithAttribute { get; set; }

            [JsonIgnore]
            public string JsonIgnored { get; set; }

            public string DataMemberIgnored { get; set; }
        }
    }
}
