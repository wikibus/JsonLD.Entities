using System;
using System.Diagnostics.CodeAnalysis;
using JsonLD.Entities.Converters;
using Newtonsoft.Json;
using NullGuard;

namespace JsonLD.Entities
{
    /// <summary>
    /// Represents a Uri reference, which will force an expanded form
    /// when JSON-LD is serialized
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Ignore equality memebers")]
    [JsonConverter(typeof(IriRefConverter))]
    public struct IriRef
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IriRef"/> struct.
        /// </summary>
        /// <param name="value">The URI.</param>
        public IriRef(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IriRef"/> struct.
        /// </summary>
        /// <param name="uri">The identifier.</param>
        public IriRef(Uri uri)
            : this(uri.ToString())
        {
        }

        /// <summary>
        /// Gets the URI.
        /// </summary>
        [JsonProperty(JsonLdKeywords.Id)]
        public string Value { [return: AllowNull] get; }

#pragma warning disable SA1600 // Elements must be documented
#pragma warning disable 1591
        public static explicit operator IriRef([AllowNull] Uri uri)
        {
            if (uri == null)
            {
                return default(IriRef);
            }

            return new IriRef(uri);
        }

        public static explicit operator IriRef([AllowNull] string uriString)
        {
            if (uriString == null)
            {
                return default(IriRef);
            }

            return new IriRef(uriString);
        }

        public static bool operator ==(IriRef left, IriRef right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IriRef left, IriRef right)
        {
            return !left.Equals(right);
        }

        public bool Equals(IriRef other)
        {
            return string.Equals(this.Value, other.Value);
        }

        public override bool Equals([AllowNull] object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is IriRef && this.Equals((IriRef)obj);
        }

        public override int GetHashCode()
        {
            return this.Value?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            if (this.Value != null)
            {
                return this.Value;
            }

            return string.Empty;
        }
#pragma warning restore SA1600 // Elements must be documented
#pragma warning restore 1591
    }
}
