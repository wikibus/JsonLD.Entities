using FakeItEasy;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Vocab;

namespace JsonLD.Entities.Tests
{
    [TestFixture]
    public class ContextResolverTests
    {
        private static readonly JToken TestContextJson;
        private ContextResolver _resolver;

        static ContextResolverTests()
        {
            TestContextJson = JToken.Parse(string.Format("{{ 'xsd': '{0}' }}", Xsd.BaseUri));
        }

        [SetUp]
        public void Setup()
        {
            _resolver = new ContextResolver(A.Fake<IContextProvider>());
        }

        [Test]
        public void Should_get_context_using_property_from_generic_type()
        {
            // when
            JToken context = _resolver.GetContext(typeof(GenericType<>));

            // thne
            Assert.That(JToken.DeepEquals(context, TestContextJson));
        }

        [Test]
        public void Should_get_context_using_property_from_closed_generic_type()
        {
            // when
            JToken context = _resolver.GetContext(typeof(GenericType<int>));

            // thne
            Assert.That(JToken.DeepEquals(context, TestContextJson));
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
