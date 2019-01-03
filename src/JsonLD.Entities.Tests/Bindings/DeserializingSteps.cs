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
        private static readonly MethodInfo DeserializeQuadsMethod;

        private static readonly MethodInfo DeserializeJsonMethod;

        private readonly SerializerTestContext testContext;
        private readonly ScenarioContext scenarioContext;

        static DeserializingSteps()
        {
            DeserializeQuadsMethod = typeof(IEntitySerializer).GetMethod("Deserialize", new[] { typeof(string) });
            DeserializeJsonMethod = typeof(IEntitySerializer).GetMethod("Deserialize", new[] { typeof(JToken) });
        }

        public DeserializingSteps(SerializerTestContext context, ScenarioContext scenarioContext)
        {
            this.testContext = context;
            this.scenarioContext = scenarioContext;
        }

        [Given(@"@context is:")]
        public void GivenContextIs(string jsonLdContext)
        {
            scenarioContext.Set(JObject.Parse(jsonLdContext), "@context");
        }

        [Given(@"frame is")]
        public void GivenFrameIs(string inputFrame)
        {
            scenarioContext.Set(JObject.Parse(inputFrame), "frame");
        }

        [Given(@"NQuads:")]
        public void GivenRDFData(string nQuads)
        {
            this.testContext.NQuads = nQuads;
        }

        [Given(@"JSON-LD:")]
        public void GivenJsonLd(string jsonLd)
        {
            this.testContext.JsonLdObject = JToken.Parse(jsonLd);
        }

        [Scope(Tag = "NQuads")]
        [When(@"I deserialize into '(.*)'")]
        public void WhenIDeserializeNQuads(string typeName)
        {
            var entityType = Type.GetType(typeName, true);

            this.SetupProviders(entityType);
            var typedDeserialize = DeserializeQuadsMethod.MakeGenericMethod(entityType);

            var entity = typedDeserialize.Invoke(this.testContext.Serializer, new object[] { this.testContext.NQuads });

            scenarioContext.Set(entity, "Entity");
        }

        [Scope(Tag = "JsonLD")]
        [When(@"I deserialize into '(.*)'")]
        public void WhenIDeserializeInto(string typeName)
        {
            var entityType = Type.GetType(typeName, true);

            this.SetupProviders(entityType);
            var typedDeserialize = DeserializeJsonMethod.MakeGenericMethod(entityType);

            var entity = typedDeserialize.Invoke(this.testContext.Serializer, new object[] { this.testContext.JsonLdObject });

            scenarioContext.Set(entity, "Entity");
        }

        private void SetupProviders(Type entityType)
        {
            JObject context = null;
            JObject frame = null;

            if (scenarioContext.ContainsKey("@context"))
            {
                context = scenarioContext.Get<JObject>("@context");
            }

            if (scenarioContext.ContainsKey("frame"))
            {
                frame = scenarioContext.Get<JObject>("frame");
            }

            A.CallTo(() => this.testContext.ContextProvider.GetContext(entityType)).Returns(context);
            A.CallTo(() => this.testContext.FrameProvider.GetFrame(entityType)).Returns(frame);
        }
    }
}
