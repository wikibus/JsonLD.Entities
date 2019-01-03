using System;
using FakeItEasy;
using JsonLD.Entities.Tests.ContextTestEntities;
using JsonLD.Entities.Tests.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace JsonLD.Entities.Tests
{
    public class EntitySerializerTests
    {
        private readonly IContextProvider provider;
        private readonly EntitySerializer serializer;

        public EntitySerializerTests()
        {
            this.provider = A.Fake<IContextProvider>();
            this.serializer = new EntitySerializer(this.provider);
        }

        [Fact]
        public void Deserializing_quads_should_throw_when_context_isnt_found()
        {
            // given
            A.CallTo(() => this.provider.GetContext(typeof(Person))).Returns(null);

            // when
            Assert.Throws<ContextNotFoundException>(() => this.serializer.Deserialize<Person>(string.Empty));
        }

        [Fact]
        public void Should_serialize_id_as_string()
        {
            // given
            A.CallTo(() => this.provider.GetContext(typeof(Person))).Returns(JObject.Parse(@"{
                'foaf': 'http://xmlns.com/foaf/0.1/',
                'name': 'foaf:givenName',
                'surname': 'foaf:familyName',
                'birthDate': 'http://example.com/ontology#dateOfBirth'
            }"));
            var id = new Uri("http://example.org/Some/Person");

            // when
            var person = this.serializer.Serialize(new Person { Id = id });

            // then
            var jId = Assert.IsType<JValue>(person["@id"]);
            Assert.Equal("http://example.org/Some/Person", jId.Value);
        }

        [Fact]
        public void Should_use_GetContext_method_from_base_class()
        {
            // given
            var obj = new DerivedClass { Name = "Tomasz" };

            // when
            var serialized = this.serializer.Serialize(obj);

            // then
            Assert.NotNull(serialized["@context"]);
        }

        [Fact]
        public void Should_compact_when_one_of_contexts_is_empty()
        {
            // given
            A.CallTo(() => this.provider.GetContext(typeof(DerivedClass))).Returns(new JArray
            {
                new JObject { { "foaf", "http://not.foaf" } },
                new JObject()
            });
            var obj = new DerivedClass { Name = "Tomasz" };

            // when
            var serialized = this.serializer.Serialize(obj, new SerializationOptions { SerializeCompacted = true });

            // then
            Assert.NotNull(serialized["@context"]);
        }

        [Theory]
        [InlineData("{ 'property': 'http://example.com/absolute/id' }")]
        [InlineData("{ 'property': '/relative/id' }")]
        public void Should_deserialize_compacted_IriRef(string json)
        {
            // given
            dynamic raw = JsonConvert.DeserializeObject(json);
            var iriRef = new IriRef(raw.property.ToString());

            // when
            var deserialized = this.serializer.Deserialize<ClassWithSomeUris>((JToken)raw);

            // then
            Assert.Equal(iriRef, deserialized.Property);
        }

        [Theory]
        [InlineData("{ 'property': { '@id': 'http://example.com/absolute/id' } }")]
        [InlineData("{ 'property': { '@id': '/relative/id' } }")]
        public void Should_deserialize_expanded_IriRef(string json)
        {
            // given
            dynamic raw = JsonConvert.DeserializeObject(json);
            var iriRef = new IriRef(raw.property["@id"].ToString());

            // when
            var deserialized = this.serializer.Deserialize<ClassWithSomeUris>((JToken)raw);

            // then
            Assert.Equal(iriRef, deserialized.Property);
        }

        [Fact]
        public void Should_deserialize_null_IriRef()
        {
            // given
            dynamic raw = JsonConvert.DeserializeObject("{ 'property': { '@id': null } }");

            // when
            var deserialized = this.serializer.Deserialize<ClassWithSomeUris>((JToken)raw);

            // then
            Assert.Equal(default(IriRef), deserialized.Property);
        }

        [Fact]
        public void Should_deserialize_null_Uri()
        {
            // given
            dynamic raw = JsonConvert.DeserializeObject("{ 'uriProperty': null }");

            // when
            var deserialized = this.serializer.Deserialize<ClassWithSomeUris>((JToken)raw);

            // then
            Assert.Null(deserialized.UriProperty);
        }

        [Fact]
        public void Should_deserialize_when_entity_serializer_was_created_with_paremterless_constructor()
        {
            // given
            dynamic raw = JsonConvert.DeserializeObject("{ 'uriProperty': null }");

            // when
            var deserialized = new EntitySerializer().Deserialize<ClassWithSomeUris>((JToken)raw);

            // then
            Assert.Null(deserialized.UriProperty);
        }
    }
}
