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
        private static readonly MethodInfo DeserializeQuadsMethod = Info.OfMethod("JsonLD.Entities", "JsonLD.Entities.IEntitySerializer", "Deserialize", "System.String");
        private static readonly MethodInfo DeserializeJsonMethod = Info.OfMethod("JsonLD.Entities", "JsonLD.Entities.IEntitySerializer", "Deserialize", "Newtonsoft.Json.Linq.JObject");
        private readonly DeserializationContext _context;

        public DeserializingSteps(DeserializationContext context)
        {
            _context = context;
        }

        [Given(@"@context is:")]
        public void GivenContextIs(string jsonLdContext)
        {
            ScenarioContext.Current.Set(JObject.Parse(jsonLdContext));
        }

        [Given(@"NQuads:")]
        public void GivenRDFData(string nQuads)
        {
            _context.NQuads = nQuads;
        }

        [Given(@"JSON-LD:")]
        public void GivenJsonLd(string jsonLd)
        {
            _context.JsonLdObject = JObject.Parse(jsonLd);
        }

        [Scope(Tag = "NQuads")]
        [When(@"I deserialize into '(.*)'")]
        public void WhenIDeserializeNQuads(string typeName)
        {
            var entityType = Type.GetType(typeName, true);

            A.CallTo(() => _context.ContextProvider.GetContext(entityType)).Returns(ScenarioContext.Current.Get<JObject>());
            var typedDeserialize = DeserializeQuadsMethod.MakeGenericMethod(entityType);

            var entity = typedDeserialize.Invoke(_context.Serializer, new object[] { _context.NQuads });

            ScenarioContext.Current.Set(entity, "Entity");
        }

        [Scope(Tag = "JsonLD")]
        [When(@"I deserialize into '(.*)'")]
        public void WhenIDeserializeInto(string typeName)
        {
            var entityType = Type.GetType(typeName, true);

            A.CallTo(() => _context.ContextProvider.GetContext(entityType)).Returns(ScenarioContext.Current.Get<JObject>());
            var typedDeserialize = DeserializeJsonMethod.MakeGenericMethod(entityType);

            var entity = typedDeserialize.Invoke(_context.Serializer, new object[] { _context.JsonLdObject });

            ScenarioContext.Current.Set(entity, "Entity");
        }
    }
}
