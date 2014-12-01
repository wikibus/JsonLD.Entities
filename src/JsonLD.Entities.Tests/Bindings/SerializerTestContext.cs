using FakeItEasy;
using JsonLD.Entities.Tests.Entities;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities.Tests.Bindings
{
    public class SerializerTestContext
    {
        private readonly IContextProvider _contextProvider;
        private readonly EntitySerializer _serializer;

        public SerializerTestContext()
        {
            _contextProvider = A.Fake<IContextProvider>();
            _serializer = new EntitySerializer(_contextProvider);
        }

        public IContextProvider ContextProvider
        {
            get { return _contextProvider; }
        }

        public EntitySerializer Serializer
        {
            get { return _serializer; }
        }

        public string NQuads { get; set; }

        public JObject JsonLdObject { get; set; }

        public object Object { get; set; }
    }
}
