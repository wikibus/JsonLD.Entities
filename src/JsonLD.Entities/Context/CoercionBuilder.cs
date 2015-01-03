using System;

namespace JsonLD.Entities.Context
{
    /// <summary>
    /// Used to create properties with type coercion
    /// </summary>
    /// <remarks>See http://www.w3.org/TR/json-ld/#type-coercion</remarks>
    public class CoercionBuilder
    {
        /// <summary>
        /// Property values should be resolved as compact IRI, absolute IRI or relative IRI
        /// </summary>
        public PropertyBuilder Id()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Property values should be resolved as vocabulary term, compact IRI, absolute IRI or relative IRI
        /// </summary>
        public PropertyBuilder Vocab()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Property values should be interpreted as typed literals
        /// </summary>
        public PropertyBuilder Is(string dataType)
        {
            throw new NotImplementedException();
        }
    }
}
