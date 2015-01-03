using Newtonsoft.Json.Linq;
using NullGuard;

namespace JsonLD.Entities.Context
{
    /// <summary>
    /// Used to create properties with type coercion
    /// </summary>
    /// <remarks>See http://www.w3.org/TR/json-ld/#type-coercion</remarks>
    [NullGuard(ValidationFlags.All)]
    public class CoercionBuilder
    {
        private readonly JProperty _property;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoercionBuilder"/> class.
        /// </summary>
        internal CoercionBuilder(JProperty property)
        {
            _property = property.EnsureExpandedDefinition();
        }

        /// <summary>
        /// Property values should be resolved as compact IRI, absolute IRI or relative IRI
        /// </summary>
        public PropertyBuilder Id()
        {
            return new PropertyBuilder(_property.With(JsonLdKeywords.Type, JsonLdKeywords.Id));
        }

        /// <summary>
        /// Property values should be resolved as vocabulary term, compact IRI, absolute IRI or relative IRI
        /// </summary>
        public PropertyBuilder Vocab()
        {
            return new PropertyBuilder(_property.With(JsonLdKeywords.Type, JsonLdKeywords.Vocab));
        }

        /// <summary>
        /// Property values should be interpreted as typed literals
        /// </summary>
        public PropertyBuilder Is(string dataType)
        {
            return new PropertyBuilder(_property.With(JsonLdKeywords.Type, dataType));
        }
    }
}
