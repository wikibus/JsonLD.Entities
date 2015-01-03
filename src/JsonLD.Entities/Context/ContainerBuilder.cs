using Newtonsoft.Json.Linq;
using NullGuard;

namespace JsonLD.Entities.Context
{
    /// <summary>
    /// Used to define property's @container
    /// </summary>
    [NullGuard(ValidationFlags.All)]
    public class ContainerBuilder
    {
        private readonly JProperty _property;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerBuilder"/> class.
        /// </summary>
        internal ContainerBuilder(JProperty property)
        {
            _property = property.EnsureExpandedDefinition();
        }

        /// <summary>
        /// The property is a @set container
        /// </summary>
        public PropertyBuilder Set()
        {
            return new PropertyBuilder(_property.With(JsonLdKeywords.Container, JsonLdKeywords.Set));
        }

        /// <summary>
        /// The property is a @list container
        /// </summary>
        public PropertyBuilder List()
        {
            return new PropertyBuilder(_property.With(JsonLdKeywords.Container, JsonLdKeywords.List));
        }

        /// <summary>
        /// The property is a @index container
        /// </summary>
        public PropertyBuilder Index()
        {
            return new PropertyBuilder(_property.With(JsonLdKeywords.Container, JsonLdKeywords.Index));
        }

        /// <summary>
        /// The property is a @language map
        /// </summary>
        public PropertyBuilder Language()
        {
            return new PropertyBuilder(_property.With(JsonLdKeywords.Container, JsonLdKeywords.Language));
        }
    }
}
