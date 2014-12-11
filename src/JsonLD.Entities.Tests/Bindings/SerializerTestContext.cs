using FakeItEasy;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities.Tests.Bindings
{
    public class SerializerTestContext
    {
        private readonly IContextProvider _contextProvider;
        private readonly EntitySerializer _serializer;
        private readonly IFrameProvider _frameProvider;

        public SerializerTestContext()
        {
            _contextProvider = A.Fake<IContextProvider>();
            _frameProvider = A.Fake<IFrameProvider>();
            _serializer = new EntitySerializer(_contextProvider, _frameProvider);
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

        public JToken JsonLdObject { get; set; }

        public object Object { get; set; }

        public IFrameProvider FrameProvider
        {
            get { return _frameProvider; }
        }
    }
}
