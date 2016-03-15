using System;
using NUnit.Framework;

namespace JsonLD.Entities.Tests
{
    [TestFixture]
    public class IriRefTests
    {
        [Test]
        public void Should_work_with_relative_Uri_string()
        {
            // given
            const string path = "/relative/path";

            // when
            var iriRef = new IriRef(path);

            // then
            Assert.That(iriRef.Value, Is.EqualTo(new Uri(path, UriKind.Relative)));
        }

        [Test]
        public void Should_work_with_absolute_Uri_string()
        {
            // given
            const string path = "http://example.com/relative/path";

            // when
            var iriRef = new IriRef(path);

            // then
            Assert.That(iriRef.Value, Is.EqualTo(new Uri(path, UriKind.Absolute)));
        }

        [Test]
        public void Two_instances_should_be_equal_when_Uri_is_equal()
        {
            // given
            const string path = "http://example.com/relative/path";

            // then
            Assert.True(new IriRef(path) == new IriRef(path));
            Assert.True(new IriRef(path).Equals(new IriRef(path)));
        }
    }
}