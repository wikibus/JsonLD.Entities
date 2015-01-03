using System;

namespace JsonLD.Entities.Context
{
    /// <summary>
    /// Used to build property maps for JSON-LD @context
    /// </summary>
    public class PropertyBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBuilder"/> class.
        /// </summary>
        public PropertyBuilder(string property, string uriOrPrefixedName)
        {
        }

        /// <summary>
        /// Creates a builder for properties with type coercion
        /// </summary>
        public CoercionBuilder Type()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a builder for properties, which are @containers
        /// </summary>
        public ContainerBuilder Container()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Defines an internationalized property
        /// </summary>
        public PropertyBuilder Language(string languageCode)
        {
            throw new NotImplementedException();
        }
    }
}
