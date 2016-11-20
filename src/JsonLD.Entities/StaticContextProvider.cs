using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NullGuard;

namespace JsonLD.Entities
{
    /// <summary>
    /// Provides a predefined @context for each model type.
    /// </summary>
    public class StaticContextProvider : IContextProvider
    {
        private readonly IDictionary<Type, JToken> contexts;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticContextProvider"/> class.
        /// </summary>
        public StaticContextProvider()
        {
            this.contexts = new Dictionary<Type, JToken>();
        }

        /// <summary>
        /// Gets the expanded context for a give serialized type..
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>JSON object or null if @context not found</returns>
        [return: AllowNull]
        public JToken GetContext(Type modelType)
        {
            JToken context;
            this.contexts.TryGetValue(modelType, out context);
            return context;
        }

        /// <summary>
        /// Sets @context used by given <paramref name="type"/>.
        /// </summary>
        public void SetContext(Type type, JToken context)
        {
            this.contexts[type] = context;
        }
    }
}
