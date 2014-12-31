using FakeItEasy;
using NUnit.Framework;

namespace JsonLD.Entities.Tests
{
    [TestFixture]
    public class ContextResolverTests
    {
        private ContextResolver _contextResolver;
        private IContextProvider _contextProvider;

        [SetUp]
        public void Setup()
        {
            _contextProvider = A.Fake<IContextProvider>();
            _contextResolver = new ContextResolver(_contextProvider);
        }
    }
}
