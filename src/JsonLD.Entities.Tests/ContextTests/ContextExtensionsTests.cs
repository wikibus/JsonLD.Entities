using JsonLD.Entities.Context;
using Newtonsoft.Json.Linq;
using Xunit;

namespace JsonLD.Entities.Tests.ContextTests
{
    public class ContextExtensionsTests
    {
        [Fact]
        public void Should_combine_multiple_objects_into_an_array()
        {
            // given
            var context1 = JObject.Parse("{ 'xsd': 'http://xsd.ns' }");
            var context2 = JObject.Parse("{ 'foaf': 'http://foaf.ns' }");
            var expected = JToken.Parse("[ { 'xsd': 'http://xsd.ns' }, { 'foaf': 'http://foaf.ns' } ]");

            // when
            var merged = context1.MergeWith(context2);

            // then
            Assert.True(JToken.DeepEquals(merged, expected));
        }

        [Fact]
        public void Should_return_original_if_no_additional_contexts_passed()
        {
            // given
            var context = JObject.Parse("{ 'xsd': 'http://xsd.ns' }");

            // when
            var merged = context.MergeWith();

            // then
            Assert.Same(context, merged);
        }

        [Fact]
        public void Should_return_original_if_nulls_are_passed()
        {
            // given
            var context = JObject.Parse("{ 'xsd': 'http://xsd.ns' }");

            // when
            var merged = context.MergeWith(null, null);

            // then
            Assert.Same(context, merged);
        }

        [Fact]
        public void Should_flatten_additonal_array_contexts()
        {
            // given
            var context1 = JObject.Parse("{ 'xsd': 'http://xsd.ns' }");
            var context2 = JToken.Parse("[ { 'sch': 'http://schema.org' }, { 'foaf': 'http://foaf.ns' } ]");
            var expected = JToken.Parse("[ { 'xsd': 'http://xsd.ns' }, { 'sch': 'http://schema.org' }, { 'foaf': 'http://foaf.ns' } ]");

            // when
            var merged = context1.MergeWith(context2);

            // then
            Assert.True(JToken.DeepEquals(merged, expected));
        }

        [Fact]
        public void Should_append_to_original_if_it_is_array()
        {
            // given
            var context1 = JToken.Parse("[ { 'sch': 'http://schema.org' }, { 'foaf': 'http://foaf.ns' } ]");
            var context2 = JObject.Parse("{ 'xsd': 'http://xsd.ns' }");
            var expected = JToken.Parse("[ { 'sch': 'http://schema.org' }, { 'foaf': 'http://foaf.ns' }, { 'xsd': 'http://xsd.ns' } ]");

            // when
            var merged = context1.MergeWith(context2);

            // then
            Assert.True(JToken.DeepEquals(merged, expected));
        }

        [Fact]
        public void Should_merge_multiple_arrays()
        {
            // given
            var context1 = JToken.Parse("[ { 'sch': 'http://schema.org' }, { 'foaf': 'http://foaf.ns' } ]");
            var context2 = JToken.Parse("[ { 'xsd': 'http://xsd.ns' }, { 'bibo': 'http://bibo.ns' } ]");
            var context3 = JToken.Parse("[ { 'dc': 'http://dc.terms' }, { 'owl': 'http://owl.ns' } ]");
            var expected = JToken.Parse(@"
[
    { 'sch': 'http://schema.org' },
    { 'foaf': 'http://foaf.ns' },
    { 'xsd': 'http://xsd.ns' },
    { 'bibo': 'http://bibo.ns' },
    { 'dc': 'http://dc.terms' },
    { 'owl': 'http://owl.ns' }
]");

            // when
            var merged = context1.MergeWith(context2, context3);

            // then
            Assert.True(JToken.DeepEquals(merged, expected));
        }

        [Fact]
        public void Should_merge_external_contexts()
        {
            // given
            var context1 = JToken.Parse("[ { 'sch': 'http://schema.org' }, { 'foaf': 'http://foaf.ns' } ]");
            var context2 = JToken.Parse("'http://external.ctx'");
            var context3 = JToken.Parse("[ { 'dc': 'http://dc.terms' }, { 'owl': 'http://owl.ns' } ]");
            var expected = JToken.Parse(@"
[
    { 'sch': 'http://schema.org' },
    { 'foaf': 'http://foaf.ns' },
    'http://external.ctx',
    { 'dc': 'http://dc.terms' },
    { 'owl': 'http://owl.ns' }
]");

            // when
            var merged = context1.MergeWith(context2, context3);

            // then
            Assert.True(JToken.DeepEquals(merged, expected));
        }
    }
}
