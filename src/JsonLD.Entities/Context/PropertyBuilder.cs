using Newtonsoft.Json.Linq;
using NullGuard;

namespace JsonLD.Entities.Context
{
    /// <summary>
    /// Used to build property maps for JSON-LD @context
    /// </summary>
    [NullGuard(ValidationFlags.All)]
    public class PropertyBuilder : JProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBuilder"/> class.
        /// </summary>
        public PropertyBuilder(string property, string uriOrPrefixedName)
            : base(property, uriOrPrefixedName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBuilder"/> class.
        /// </summary>
        internal PropertyBuilder(JProperty property)
            : base(property)
        {
        }

        /// <summary>
        /// Creates a builder for properties with type coercion
        /// </summary>
        public new CoercionBuilder Type()
        {
            return new CoercionBuilder(this);
        }

        /// <summary>
        /// Creates a builder for properties, which are @containers
        /// </summary>
        public ContainerBuilder Container()
        {
            return new ContainerBuilder(this);
        }

        /// <summary>
        /// Defines an internationalized property
        /// </summary>
        public PropertyBuilder Language([AllowNull] string languageCode)
        {
            var property = this.EnsureExpandedDefinition()
                               .With(JsonLdKeywords.Language, languageCode);
            return new PropertyBuilder(property);
        }
    }
}
