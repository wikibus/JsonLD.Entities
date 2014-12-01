using System;
using FakeItEasy;
using JsonLD.Entities.Tests.Entities;
using JsonLD.Entities.Tests.Helpers;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace JsonLD.Entities.Tests.Bindings
{
    [Binding]
    public class SerializingSteps
    {
        private readonly SerializerTestContext _context;

        public SerializingSteps(SerializerTestContext context)
        {
            _context = context;
            A.CallTo(() => _context.ContextProvider.GetContext(typeof(Person))).Returns(Default.PersonContext);
        }

        [Given(@"a person without id")]
        public void GivenAPersonWithoutId()
        {
            _context.Object = new Person
                {
                    Name = "Tomasz",
                    Surname = "Pluskiewicz",
                    BirthDate = new DateTime(1972, 9, 4)
                };
        }

        [When(@"the object is serialized")]
        public void WhenTheObjectIsSerialized()
        {
            _context.JsonLdObject = _context.Serializer.Serialize(_context.Object);
        }

        [Then(@"the resulting JSON-LD should be:")]
        public void ThenTheResultingJsonLdShouldBe(string jObject)
        {
            var expected = JObject.Parse(jObject);
            Assert.That(JToken.DeepEquals(_context.JsonLdObject, expected));
        }
    }
}
