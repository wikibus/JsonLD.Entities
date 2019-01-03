using FakeItEasy;
using Newtonsoft.Json.Linq;
using Vocab;
using Xunit;

namespace JsonLD.Entities.Tests
{
    public class ContextResolverTests
    {
        private static readonly JToken TestContextJson;
        private readonly ContextResolver resolver;

        static ContextResolverTests()
        {
            TestContextJson = JToken.Parse(string.Format("{{ 'xsd': '{0}' }}", Xsd.BaseUri));
        }

        public ContextResolverTests()
        {
            this.resolver = new ContextResolver(A.Fake<IContextProvider>());
        }

        [Fact]
        public void Should_get_context_using_property_from_generic_type()
        {
            // when
            JToken context = this.resolver.GetContext(typeof(GenericType<>));

            // then
            Assert.True(JToken.DeepEquals(context, TestContextJson));
        }

        [Fact]
        public void Should_get_context_using_property_from_closed_generic_type()
        {
            // when
            JToken context = this.resolver.GetContext(typeof(GenericType<int>));

            // then
            Assert.True(JToken.DeepEquals(context, TestContextJson));
        }

        [Fact]
        public void Should_prefer_method_over_property()
        {
            // when
            JToken context = this.resolver.GetContext(typeof(MethodAndProperty));

            // then
            Assert.True(JToken.Equals(context, (JToken)"method"));
        }

        private class MethodAndProperty
        {
            private static JToken Context
            {
                get
                {
                    return "property";
                }
            }

            private static JToken GetContext(MethodAndProperty _)
            {
                 return "method";
            }
        }

        private class GenericType<T>
        {
            private static JToken Context
            {
                get
                {
                    return TestContextJson;
                }
            }
        }
    }
}
