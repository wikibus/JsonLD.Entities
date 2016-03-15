using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using NullGuard;

namespace JsonLD.Entities
{
    /// <summary>
    /// Represents a Uri reference, which will force an expanded form
    /// when JSON-LD is serialized
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Ignore equality memebers")]
    public struct IriRef
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IriRef"/> struct.
        /// </summary>
        /// <param name="uri">The URI.</param>
        public IriRef(string uri) : this(new Uri(uri, UriKind.RelativeOrAbsolute))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IriRef"/> struct.
        /// </summary>
        /// <param name="uri">The identifier.</param>
        public IriRef(Uri uri)
        {
            Value = uri;
        }

        /// <summary>
        /// Gets the URI.
        /// </summary>
        [JsonProperty(JsonLdKeywords.Id)]
        public Uri Value { [return: AllowNull] get; }

        public static bool operator ==(IriRef left, IriRef right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IriRef left, IriRef right)
        {
            return !left.Equals(right);
        }
    }
}