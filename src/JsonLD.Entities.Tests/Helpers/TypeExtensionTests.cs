using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JsonLD.Entities.Tests.Helpers
{
    public class TypeExtensionTests
    {
        [Test]
        public void Should_return_type_from_string_Type_property()
        {
            // given
            var expected = new Uri("http://example.com/vocab/TypeAsString");

            // when
            var typeIdentifier = typeof(TypeAsString).GetTypeIdentifier();

            // then
            Assert.That(typeIdentifier, Is.EqualTo(expected));
        }

        [Test]
        public void Should_return_type_from_Uri_Type_property()
        {
            // given
            var expected = new Uri("http://example.com/vocab/TypeAsUri");

            // when
            var typeIdentifier = typeof(TypeAsUri).GetTypeIdentifier();

            // then
            Assert.That(typeIdentifier, Is.EqualTo(expected));
        }

        [Test]
        public void Should_return_type_from_Uri_Types_property()
        {
            // given
            var expected = new Uri("http://example.com/vocab/TypesAsUri");

            // when
            var typeIdentifier = typeof(TypesAsUri).GetTypeIdentifier();

            // then
            Assert.That(typeIdentifier, Is.EqualTo(expected));
        }

        [Test]
        public void Should_return_type_from_string_Types_property()
        {
            // given
            var expected = new Uri("http://example.com/vocab/TypesAsString");

            // when
            var typeIdentifier = typeof(TypesAsString).GetTypeIdentifier();

            // then
            Assert.That(typeIdentifier, Is.EqualTo(expected));
        }

        [Test]
        public void Should_return_type_from_annotated_property()
        {
            // given
            var expected = new Uri("http://example.com/vocab/CustomProperty");

            // when
            var typeIdentifier = typeof(CustomProperty).GetTypeIdentifier();

            // then
            Assert.That(typeIdentifier, Is.EqualTo(expected));
        }

        [Test]
        public void Should_throw_when_Types_contains_multiple_elements()
        {
            Assert.Throws<InvalidOperationException>(() => typeof(MultipleTypesAsUri).GetTypeIdentifier());
        }

        [Test]
        public void Should_throw_when_class_doesnt_have_a_static_types_property()
        {
            Assert.Throws<InvalidOperationException>(() => typeof(object).GetTypeIdentifier());
        }

        private class TypeAsString
        {
            private static string Type => "http://example.com/vocab/TypeAsString";
        }

        private class TypeAsUri
        {
            public static Uri Type => new Uri("http://example.com/vocab/TypeAsUri");
        }

        private class TypesAsUri
        {
            public static Uri[] Types => new[]
            {
                new Uri("http://example.com/vocab/TypesAsUri")
            };
        }

        private class TypesAsString
        {
            public static IList<string> Types => new List<string>
            {
                "http://example.com/vocab/TypesAsString"
            };
        }

        private class MultipleTypesAsUri
        {
            public static IEnumerable<Uri> Types
            {
                get
                {
                    yield return new Uri("http://example.com/vocab/MultipleTypesAsUri");
                    yield return new Uri("http://example.com/vocab/SecondType");
                }
            }
        }

        private class CustomProperty
        {
            [JsonProperty(JsonLdKeywords.Type)]
            private static string Class => "http://example.com/vocab/CustomProperty";
        }
    }
}