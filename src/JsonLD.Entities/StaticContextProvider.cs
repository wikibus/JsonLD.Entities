using System;
using Newtonsoft.Json.Linq;
using NullGuard;

namespace JsonLD.Entities
{
    /// <summary>
    /// Provides a predefined @context for each model type.
    /// </summary>
    public class StaticContextProvider : IContextProvider
    {
        /// <summary>
        /// Gets the expanded context for a give serialized type..
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>JSON object or null if @context not found</returns>
        [return: AllowNull]
        public JObject GetContext(Type modelType)
        {
            return null;
        }
    }
}