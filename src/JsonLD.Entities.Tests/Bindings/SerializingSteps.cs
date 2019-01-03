using System;
using Dynamitey;
using FakeItEasy;
using JsonLD.Entities.Tests.Entities;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;
using Xunit;

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
                    BirthDate = new DateTime(1972, 9, 4),
                    Age = 30
                };
        }

        [Given(@"model of type '(.*)'")]
        public void GivenModelOfType(string typeName)
        {
            var model = Type.GetType(typeName, true);
            this.context.Object = Activator.CreateInstance(model);
        }

        [Given(@"model has interest '(.*)'")]
        public void GivenModelInterestsRDF(string value)
        {
            Dynamic.InvokeMemberAction(this.context.Object, "AddInterest", value);
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
            Assert.True(JToken.DeepEquals(this.context.JsonLdObject, expected), $"Actual object was: {this.context.JsonLdObject}");
        }
    }
}
