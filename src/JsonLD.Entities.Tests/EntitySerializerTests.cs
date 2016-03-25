using System;
using FakeItEasy;
using JsonLD.Entities.Tests.ContextTestEntities;
using JsonLD.Entities.Tests.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
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
        public void Should_use_GetContext_method_from_base_class()
        {
            // given
            var obj = new DerivedClass { Name = "Tomasz" };

            // when
            var serialized = _serializer.Serialize(obj);

            // then
            Assert.That(serialized["@context"], Is.Not.Null);
        }

        [Test]
        public void Should_compact_when_one_of_contexts_is_empty()
        {
            // given
            A.CallTo(() => _provider.GetContext(typeof(DerivedClass))).Returns(new JArray
            {
                new JObject { { "foaf", "http://not.foaf" } },
                new JObject()
            });
            var obj = new DerivedClass { Name = "Tomasz" };

            // when
            var serialized = _serializer.Serialize(obj, new SerializationOptions { SerializeCompacted = true });

            // then
            Assert.That(serialized["@context"], Is.Not.Null);
        }

        [Test]
        [TestCase("{ 'property': 'http://example.com/absolute/id' }", Description = "Absolute IriRef")]
        [TestCase("{ 'property': '/relative/id' }", Description = "Relative IriRef")]
        public void Should_deserialize_compacted_IriRef(string json)
        {
            // given
            dynamic raw = JsonConvert.DeserializeObject(json);
            var iriRef = new IriRef(raw.property.ToString());

            // when
            var deserialized = _serializer.Deserialize<ClassWithSomeUris>((JToken)raw);

            // then
            Assert.That(deserialized.Property, Is.EqualTo(iriRef));
        }

        [Test]
        [TestCase("{ 'property': { '@id': 'http://example.com/absolute/id' } }", Description = "Absolute IriRef")]
        [TestCase("{ 'property': { '@id': '/relative/id' } }", Description = "Relative IriRef")]
        public void Should_deserialize_expanded_IriRef(string json)
        {
            // given
            dynamic raw = JsonConvert.DeserializeObject(json);
            var iriRef = new IriRef(raw.property["@id"].ToString());

            // when
            var deserialized = _serializer.Deserialize<ClassWithSomeUris>((JToken)raw);

            // then
            Assert.That(deserialized.Property, Is.EqualTo(iriRef));
        }

        [Test]
        public void Should_deserialize_null_IriRef()
        {
            // given
            dynamic raw = JsonConvert.DeserializeObject("{ 'property': { '@id': null } }");

            // when
            var deserialized = _serializer.Deserialize<ClassWithSomeUris>((JToken)raw);

            // then
            Assert.That(deserialized.Property, Is.EqualTo(default(IriRef)));
        }

        [Test]
        public void Should_deserialize_null_Uri()
        {
            // given
            dynamic raw = JsonConvert.DeserializeObject("{ 'uriProperty': null }");

            // when
            var deserialized = _serializer.Deserialize<ClassWithSomeUris>((JToken)raw);

            // then
            Assert.That(deserialized.UriProperty, Is.Null);
        }

        [Test]
        public void Should_serialize_boolean_correctly()
        {
            // given
            var obj = new { Value = true };

            // when
            dynamic ld = _serializer.Serialize(obj);

            // then
            object value = ld.value;
            Assert.That(value.ToString(), Is.EqualTo("true"));
        }
    }
}
