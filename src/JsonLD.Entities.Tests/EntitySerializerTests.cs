using System;
using FakeItEasy;
using JsonLD.Entities.Tests.Entities;
using JsonLD.Entities.Tests.Helpers;
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
            A.CallTo(() => _provider.GetContext(typeof(Person))).Returns(Default.PersonContext);
            var id = new Uri("http://example.org/Some/Person");

            // when
            var person = _serializer.Serialize(new Person { Id = id });

            // then
            Assert.That(person["@id"], Is.InstanceOf<JValue>());
            Assert.That(person["@id"], Has.Property("Value").EqualTo("http://example.org/Some/Person"));
        }
    }
}
