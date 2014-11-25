using System;
using System.Globalization;
using System.Reflection;
using ImpromptuInterface;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace JsonLD.Entities.Tests.Bindings
{
    [Binding]
    public class DeserializingRDFDataIntoObjectsSteps
    {
        private static readonly MethodInfo DeserializeMethod = Info.OfMethod("JsonLD.Entities", "JsonLD.Entities.IEntitySerializer", "Deserialize", "System.String");

        private IEntitySerializer _serializer;
        private string _nquads;
        private object _entity;
        private IContextProvider _contextProvider;

        public DeserializingRDFDataIntoObjectsSteps()
        {
            _serializer = new EntitySerializer(_contextProvider);
        }

        [Given(@"NQuads:")]
        public void GivenRDFData(string nQuads)
        {
            _nquads = nQuads;
        }
        
        [Given(@"@context is:")]
        public void GivenContextIs(string jsonLdContext)
        {
        }
        
        [When(@"I deserialize into '(.*)'")]
        public void WhenIDeserializeInto(string typeName)
        {
            var entityType = Type.GetType(typeName, true);
            var typedDeserialize = DeserializeMethod.MakeGenericMethod(entityType);

            _entity = typedDeserialize.Invoke(_serializer, new object[] { _nquads });
        }
        
        [Then(@"object should have property '(.*)' equal to '(.*)'")]
        public void ThenObjectShouldHavePropertyEqualTo(string propertyName, string expectedValue)
        {
            var actualValue = Impromptu.InvokeGet(_entity, propertyName);
          
            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        [Then(@"object should have DateTime property '(.*)' equal to '(\d\d-\d\d-\d\d\d\d)'")]
        public void ThenObjectShouldHaveDateTimePropertyEqualTo(string propertyName, string expectedDateString)
        {
            var actualValue = Impromptu.InvokeGet(_entity, propertyName);
            var expectedValue = DateTime.ParseExact(expectedDateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }
    }
}
