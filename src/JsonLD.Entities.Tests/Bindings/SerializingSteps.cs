using System;
using FakeItEasy;
using ImpromptuInterface;
using JsonLD.Entities.Tests.Entities;
using JsonLD.Entities.Tests.SpecflowHelpers;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace JsonLD.Entities.Tests.Bindings
{
    [Binding]
    public class SerializingSteps
    {
        private readonly SerializerTestContext context;

        public SerializingSteps(SerializerTestContext context)
        {
            this.context = context;
            A.CallTo(() => context.ContextProvider.GetContext(A<Type>.Ignored)).Returns(null);
        }

        [Given(@"a person without id")]
        public void GivenAPersonWithoutId()
        {
            this.context.Object = new Person
                {
                    Name = "Tomasz",
                    Surname = "Pluskiewicz",
                    BirthDate = new DateTime(1972, 9, 4)
                };
        }

        [Given(@"model of type '(.*)'")]
        public void GivenModelOfType(string typeName)
        {
            var model = Type.GetType(typeName, true);
            this.context.Object = Activator.CreateInstance(model);
        }

        [Given(@"model of type '(.*)'")]
        public void GivenModelOfType(string typeName, Table table)
        {
            var modelType = Type.GetType(typeName, true);
            this.context.Object = table.CreateInstance(modelType);
        }

        [Given(@"model has interest '(.*)'")]
        public void GivenModelInterestsRDF(string value)
        {
            Impromptu.InvokeMemberAction(this.context.Object, "AddInterest", value);
        }

        [When(@"the object is serialized")]
        public void WhenTheObjectIsSerialized()
        {
            this.context.JsonLdObject = this.context.Serializer.Serialize(this.context.Object);
        }

        [Then(@"the resulting JSON-LD should be:")]
        public void ThenTheResultingJsonLdShouldBe(string jObject)
        {
            var expected = JObject.Parse(jObject);
            Assert.That(JToken.DeepEquals(this.context.JsonLdObject, expected), "Actual object was: {0}", this.context.JsonLdObject);
        }
    }
}
