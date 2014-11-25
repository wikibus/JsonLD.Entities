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
        private static readonly MethodInfo DeserializeMethod = Info.OfMethod("JsonLD.Entities", "JsonLD.Entities.IEntitySerializer", "Deserialize", "System.String");
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

        [Scope(Tag = "NQuads")]
        [When(@"I deserialize into '(.*)'")]
        public void WhenIDeserializeInto(string typeName)
        {
            var entityType = Type.GetType(typeName, true);

            A.CallTo(() => _context.ContextProvider.GetExpandedContext(entityType)).Returns(ScenarioContext.Current.Get<JObject>());
            var typedDeserialize = DeserializeMethod.MakeGenericMethod(entityType);

            var entity = typedDeserialize.Invoke(_context.Serializer, new object[] { _context.NQuads });

            ScenarioContext.Current.Set(entity, "Entity");
        }
    }
}
