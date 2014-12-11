using System;
using System.Reflection;
using FakeItEasy;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;

namespace JsonLD.Entities.Tests.Bindings
{
    [Binding]
    public class DeserializingSteps
    {
        private static readonly MethodInfo DeserializeQuadsMethod = Info.OfMethod("JsonLD.Entities",
                                                                                  "JsonLD.Entities.IEntitySerializer",
                                                                                  "Deserialize",
                                                                                  "System.String");

        private static readonly MethodInfo DeserializeJsonMethod = Info.OfMethod("JsonLD.Entities",
                                                                                 "JsonLD.Entities.IEntitySerializer",
                                                                                 "Deserialize",
                                                                                 "Newtonsoft.Json.Linq.JToken");

        private readonly SerializerTestContext _context;

        public DeserializingSteps(SerializerTestContext context)
        {
            _context = context;
        }

        [Given(@"@context is:")]
        public void GivenContextIs(string jsonLdContext)
        {
            ScenarioContext.Current.Set(JObject.Parse(jsonLdContext), "@context");
        }

        [Given(@"frame is")]
        public void GivenFrameIs(string inputFrame)
        {
            ScenarioContext.Current.Set(JObject.Parse(inputFrame), "frame");
        }

        [Given(@"NQuads:")]
        public void GivenRDFData(string nQuads)
        {
            _context.NQuads = nQuads;
        }

        [Given(@"JSON-LD:")]
        public void GivenJsonLd(string jsonLd)
        {
            _context.JsonLdObject = JToken.Parse(jsonLd);
        }

        [Scope(Tag = "NQuads")]
        [When(@"I deserialize into '(.*)'")]
        public void WhenIDeserializeNQuads(string typeName)
        {
            var entityType = Type.GetType(typeName, true);

            SetupProviders(entityType);
            var typedDeserialize = DeserializeQuadsMethod.MakeGenericMethod(entityType);

            var entity = typedDeserialize.Invoke(_context.Serializer, new object[] { _context.NQuads });

            ScenarioContext.Current.Set(entity, "Entity");
        }

        [Scope(Tag = "JsonLD")]
        [When(@"I deserialize into '(.*)'")]
        public void WhenIDeserializeInto(string typeName)
        {
            var entityType = Type.GetType(typeName, true);

            SetupProviders(entityType);
            var typedDeserialize = DeserializeJsonMethod.MakeGenericMethod(entityType);

            var entity = typedDeserialize.Invoke(_context.Serializer, new object[] { _context.JsonLdObject });

            ScenarioContext.Current.Set(entity, "Entity");
        }

        [Then(@"Should fail")]
        public void ThenShouldFail()
        {
            ScenarioContext.Current.Pending();
        }

        private void SetupProviders(Type entityType)
        {
            JObject @context = null;
            JObject frame = null;

            if (ScenarioContext.Current.ContainsKey("@context"))
            {
                @context = ScenarioContext.Current.Get<JObject>("@context");
            }

            if (ScenarioContext.Current.ContainsKey("frame"))
            {
                frame = ScenarioContext.Current.Get<JObject>("frame");
            }

            A.CallTo(() => _context.ContextProvider.GetContext(entityType)).Returns(@context);
            A.CallTo(() => _context.FrameProvider.GetFrame(entityType)).Returns(frame);
        }
    }
}
