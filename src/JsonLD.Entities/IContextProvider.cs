using System;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities
{
    /// <summary>
    /// Contract for classes, which provide JSON-LD @context for given types
    /// </summary>
    public interface IContextProvider
    {
        /// <summary>
        /// Gets the JSON-LD @context for a given serialized type.
        /// </summary>
        /// <param name="modelType">Type of the entity.</param>
        JToken GetContext(Type modelType);

        /// <summary>
        /// Gets the expanded context for a give serialized type..
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        JObject GetExpandedContext(Type modelType);
    }
}
