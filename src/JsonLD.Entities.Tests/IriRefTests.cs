using System;
using Xunit;

namespace JsonLD.Entities.Tests
{
    public class IriRefTests
    {
        private const string TestUri = "http://example.com/relative/path";

        [Fact]
        public void Should_work_with_relative_Uri_string()
        {
            // given
            const string path = "/relative/path";

            // when
            var iriRef = new IriRef(path);

            // then
            Assert.Equal(new Uri(path, UriKind.Relative).ToString(), iriRef.Value);
        }

        [Fact]
        public void Should_work_with_absolute_Uri_string()
        {
            // given
            const string path = TestUri;

            // when
            var iriRef = new IriRef(path);

            // then
            Assert.Equal(new Uri(path, UriKind.Absolute).ToString(), iriRef.Value);
        }

        [Fact]
        public void Two_instances_should_be_equal_when_Uri_is_equal()
        {
            // given
            const string path = TestUri;

            // then
            Assert.True(new IriRef(path) == new IriRef(path));
            Assert.True(new IriRef(path).Equals(new IriRef(path)));
        }

        [Fact]
        public void Should_be_explicitly_castable_from_Uri()
        {
            // given
            var expected = new IriRef(TestUri);

            // when
            IriRef iriRef = (IriRef)new Uri(TestUri);

            // then
            Assert.Equal(expected, iriRef);
        }

        [Fact]
        public void Should_be_explicitly_castable_from_string()
        {
            // given
            var expected = new IriRef(TestUri);

            // when
            var iriRef = (IriRef)TestUri;

            // then
            Assert.Equal(expected, iriRef);
        }

        [Fact]
        public void Should_be_explicitly_castable_from_null_string()
        {
            // given
            var expected = default(IriRef);

            // when
            var iriRef = (IriRef)(string)null;

            // then
            Assert.Equal(expected, iriRef);
        }

        [Fact]
        public void Should_be_explicitly_castable_from_null_Uri()
        {
            // given
            var expected = default(IriRef);

            // when
            var iriRef = (IriRef)(Uri)null;

            // then
            Assert.Equal(expected, iriRef);
        }
    }
}
