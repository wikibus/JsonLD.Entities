using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities.Context
{
    /// <summary>
    /// Extension to work with JSON-LD @contexts
    /// </summary>
    public static class ContextExtensions
    {
        /// <summary>
        /// Merges the multiple @contexts by combining them within a single array.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="otherContexts">The other contexts.</param>
        /// <returns>
        /// original <paramref name="context"/> or a flattened <see cref="JArray"/>
        /// </returns>
        public static JToken MergeWith(this JToken context, params JToken[] otherContexts)
        {
            otherContexts = otherContexts.Where(ctx => ctx != null).ToArray();

            if (otherContexts.Length == 0)
            {
                return context;
            }

            JToken mergedContext = context is JArray ? context : new JArray(context);

            foreach (var token in SplitTokens(otherContexts))
            {
                ((JArray)mergedContext).Add(token);
            }

            return mergedContext;
        }

        private static IEnumerable<JToken> SplitTokens(IEnumerable<JToken> otherContexts)
        {
            foreach (var context in otherContexts)
            {
                if (context is JArray)
                {
                    foreach (var inner in SplitTokens(context))
                    {
                        yield return inner;
                    }
                }
                else
                {
                    yield return context;
                }
            }
        }
    }
}
