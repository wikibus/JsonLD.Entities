using System;
using FakeItEasy;
using ImpromptuInterface;
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
            A.CallTo(() => _context.ContextProvider.GetContext(typeof(HasInterestsArray))).Returns(Default.HasInterestsSetContext);
            A.CallTo(() => _context.ContextProvider.GetContext(typeof(HasInterestsCollection))).Returns(Default.HasInterestsSetContext);
            A.CallTo(() => _context.ContextProvider.GetContext(typeof(HasInterestsEnumerable))).Returns(Default.HasInterestsSetContext);
            A.CallTo(() => _context.ContextProvider.GetContext(typeof(HasInterestsList))).Returns(Default.HasInterestsListContext);
            A.CallTo(() => _context.ContextProvider.GetContext(typeof(HasInterestsSet))).Returns(Default.HasInterestsSetContext);
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

        [Given(@"model of type '(.*)'")]
        public void GivenModelOfType(string typeName)
        {
            var model = Type.GetType(typeName, true);
            _context.Object = Activator.CreateInstance(model);
        }

        [Given(@"model has interest '(.*)'")]
        public void GivenModelInterestsRDF(string value)
        {
            Impromptu.InvokeMemberAction(_context.Object, "AddInterest", value);
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
            Assert.That(JToken.DeepEquals(_context.JsonLdObject, expected), "Actual object was: {0}", _context.JsonLdObject);
        }
    }
}
