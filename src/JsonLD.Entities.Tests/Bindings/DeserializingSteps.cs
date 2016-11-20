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

        static DeserializingSteps()
        {
            DeserializeQuadsMethod = typeof(IEntitySerializer).GetMethod("Deserialize", new[] { typeof(string) });
            DeserializeJsonMethod = typeof(IEntitySerializer).GetMethod("Deserialize", new[] { typeof(JToken) });
        }

        public DeserializingSteps(SerializerTestContext context)
        {
            this.testContext = context;
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

            ScenarioContext.Current.Set(entity, "Entity");
        }

        [Scope(Tag = "JsonLD")]
        [When(@"I deserialize into '(.*)'")]
        public void WhenIDeserializeInto(string typeName)
        {
            var entityType = Type.GetType(typeName, true);

            this.SetupProviders(entityType);
            var typedDeserialize = DeserializeJsonMethod.MakeGenericMethod(entityType);

            var entity = typedDeserialize.Invoke(this.testContext.Serializer, new object[] { this.testContext.JsonLdObject });

            ScenarioContext.Current.Set(entity, "Entity");
        }

        [Then(@"Should fail")]
        public void ThenShouldFail()
        {
            ScenarioContext.Current.Pending();
        }

        private void SetupProviders(Type entityType)
        {
            JObject context = null;
            JObject frame = null;

            if (ScenarioContext.Current.ContainsKey("@context"))
            {
                context = ScenarioContext.Current.Get<JObject>("@context");
            }

            if (ScenarioContext.Current.ContainsKey("frame"))
            {
                frame = ScenarioContext.Current.Get<JObject>("frame");
            }

            A.CallTo(() => this.testContext.ContextProvider.GetContext(entityType)).Returns(context);
            A.CallTo(() => this.testContext.FrameProvider.GetFrame(entityType)).Returns(frame);
        }
    }
}
