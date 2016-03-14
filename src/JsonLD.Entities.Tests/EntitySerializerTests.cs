using System;
using FakeItEasy;
using JsonLD.Entities.Context;
using JsonLD.Entities.Tests.Entities;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonLD.Entities.Tests
{
    [TestFixture]
    public class EntitySerializerTests
    {
        private IContextProvider _provider;
        private EntitySerializer _serializer;

        [SetUp]
        public void Setup()
        {
            _provider = A.Fake<IContextProvider>();
            _serializer = new EntitySerializer(_provider);
        }

        [Test]
        [ExpectedException(typeof(ContextNotFoundException))]
        public void Deserializing_quads_should_throw_when_context_isnt_found()
        {
            // given
            A.CallTo(() => _provider.GetContext(typeof(Person))).Returns(null);

            // when
            _serializer.Deserialize<Person>(string.Empty);
        }

        [Test]
        public void Should_serialize_id_as_string()
        {
            // given
            A.CallTo(() => _provider.GetContext(typeof(Person))).Returns(JObject.Parse(@"{
                'foaf': 'http://xmlns.com/foaf/0.1/',
                'name': 'foaf:givenName',
                'surname': 'foaf:familyName',
                'birthDate': 'http://example.com/ontology#dateOfBirth'
            }"));
            var id = new Uri("http://example.org/Some/Person");

            // when
            var person = _serializer.Serialize(new Person { Id = id });

            // then
            Assert.That(person["@id"], Is.InstanceOf<JValue>());
            Assert.That(person["@id"], Has.Property("Value").EqualTo("http://example.org/Some/Person"));
        }

        [Test]
        public void Should_ensure_all_properties_are_compacted()
        {
            // given
            A.CallTo(() => _provider.GetContext(typeof(PropertiesMappedToAbsoluteUrls)))
                .Returns(new JObject("name".IsProperty("http://xmlns.com/foaf/0.1/givenName")));

            // when
            dynamic person = _serializer.Serialize(new PropertiesMappedToAbsoluteUrls { Name = "Tomasz" });

            // then
            Assert.That(person.name.ToString(), Is.EqualTo("Tomasz"));
        }
    }
}
