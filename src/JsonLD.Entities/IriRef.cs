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
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IriRef"/> struct.
        /// </summary>
        /// <param name="uri">The identifier.</param>
        public IriRef(Uri uri) : this(uri.ToString())
        {
        }

        /// <summary>
        /// Gets the URI.
        /// </summary>
        [JsonProperty(JsonLdKeywords.Id)]
        public string Value { [return: AllowNull] get; }

        public static bool operator ==(IriRef left, IriRef right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IriRef left, IriRef right)
        {
            return !left.Equals(right);
        }

        public static implicit operator IriRef(Uri uri)
        {
            return new IriRef(uri);
        }

        public static explicit operator IriRef(string uriString)
        {
            return new IriRef(uriString);
        }

        public override string ToString()
        {
            if (Value != null)
            {
                return Value.ToString();
            }

            return string.Empty;
        }
    }
}