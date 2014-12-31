using System;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities
{
    /// <summary>
    /// Resolves a @context for entity types
    /// </summary>
    public sealed class ContextResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextResolver"/> class.
        /// </summary>
        /// <param name="contextProvider">The context provider.</param>
        public ContextResolver(IContextProvider contextProvider)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextResolver"/> class.
        /// </summary>
        public ContextResolver()
        {
        }

        /// <summary>
        /// Gets @context for an entity type.
        /// </summary>
        public JToken GetContext(Type type)
        {
            return new JObject();
        }

        /// <summary>
        /// Gets @context for an entity instance.
        /// </summary>
        public JToken GetContext(object entity)
        {
            return new JObject();
        }
    }
}
