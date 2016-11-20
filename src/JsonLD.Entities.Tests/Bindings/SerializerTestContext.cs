using FakeItEasy;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities.Tests.Bindings
{
    public class SerializerTestContext
    {
        private readonly IContextProvider contextProvider;
        private readonly EntitySerializer serializer;
        private readonly IFrameProvider frameProvider;

        public SerializerTestContext()
        {
            this.contextProvider = A.Fake<IContextProvider>();
            this.frameProvider = A.Fake<IFrameProvider>();
            this.serializer = new EntitySerializer(this.contextProvider, this.frameProvider);
        }

        public IContextProvider ContextProvider
        {
            get { return this.contextProvider; }
        }

        public EntitySerializer Serializer
        {
            get { return this.serializer; }
        }

        public string NQuads { get; set; }

        public JToken JsonLdObject { get; set; }

        public object Object { get; set; }

        public IFrameProvider FrameProvider
        {
            get { return this.frameProvider; }
        }
    }
}
