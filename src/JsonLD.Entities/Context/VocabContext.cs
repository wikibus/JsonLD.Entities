using Newtonsoft.Json.Linq;

namespace JsonLD.Entities.Context
{
    /// <summary>
    /// Automatic @context used which constructs missing property mappings
    /// by appending property names to a base vocabulary URI
    /// </summary>
    /// <typeparam name="T">Model type</typeparam>
    public class VocabContext<T> : AutoContextBase<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VocabContext{T}"/> class.
        /// </summary>
        /// <param name="baseUri">The base URI.</param>
        public VocabContext(string baseUri)
            : base(new VocabularyStrategy(baseUri))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VocabContext{T}"/> class.
        /// </summary>
        /// <param name="context">The preexisting context.</param>
        /// <param name="baseUri">The base URI.</param>
        public VocabContext(JObject context, string baseUri)
            : base(context, new VocabularyStrategy(baseUri))
        {
        }

        private class VocabularyStrategy : AutoContextStrategy
        {
            private readonly string baseUri;

            public VocabularyStrategy(string baseUri)
            {
                this.baseUri = baseUri;
            }

            protected override string GetPropertyId(string propertyName)
            {
                return this.baseUri + propertyName;
            }
        }
    }
}
