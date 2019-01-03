using System;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities.Context
{
    /// <summary>
    /// Base class for automatically creating @context for the given type
    /// </summary>
    /// <typeparam name="T">model type</typeparam>
    public abstract class AutoContextBase<T> : JObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoContextBase{T}"/> class.
        /// </summary>
        protected AutoContextBase(AutoContextStrategy strategy)
        {
            strategy.InitializeProperties(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoContextBase{T}"/> class.
        /// </summary>
        protected AutoContextBase(JObject context, AutoContextStrategy strategy)
            : base(context)
        {
            strategy.InitializeProperties(this);
        }

        /// <summary>
        /// Allows setting up additional context mapping properties for
        /// a mapping such as type coercion of language
        /// </summary>
        /// <typeparam name="TProp">The return type of property.</typeparam>
        /// <exception cref="System.ArgumentException">
        /// if selected property is not found in the @context
        /// </exception>
        public AutoContextBase<T> Property<TProp>(
            Expression<Func<T, TProp>> propertyExpression,
            Func<PropertyBuilder, JProperty> setupMapping)
        {
            var property = propertyExpression.GetProperty();
            string contextKey = property.GetJsonPropertyName();

            if (this[contextKey] == null)
            {
                var message = string.Format("Cannot find property '{0}' in @context", contextKey);
                throw new ArgumentException(message, nameof(propertyExpression));
            }

            var currentMappedPredicate = GetPropertyKey(this[contextKey]);
            if (currentMappedPredicate == null)
            {
                var message = string.Format("Invalid value in @context mapped to property {0}", contextKey);
                throw new InvalidOperationException(message);
            }

            var newMapping = setupMapping(contextKey.IsProperty(currentMappedPredicate));
            this[contextKey] = newMapping.Value;

            return this;
        }

        private static string GetPropertyKey(JToken jToken)
        {
            if (jToken is JObject)
            {
                return ((JObject)jToken)[JsonLdKeywords.Id].ToString();
            }

            if (jToken is JValue)
            {
                return jToken.ToString();
            }

            return null;
        }

        /// <summary>
        /// Implementing classes define how JSON property is
        /// translated to a predicate URI
        /// </summary>
        protected abstract class AutoContextStrategy
        {
            /// <summary>
            /// Initializes the mappings for properties unmapped in @context.
            /// </summary>
            internal void InitializeProperties(JObject context)
            {
                foreach (var property in typeof(T).GetProperties())
                {
                    var contextKey = property.GetJsonPropertyName();
                    if (JsonLdKeywords.IsKeyword(contextKey))
                    {
                        continue;
                    }

                    if (context[contextKey] == null)
                    {
                        context.Add(contextKey, this.GetPropertyId(contextKey));
                    }
                }
            }

            /// <summary>
            /// Gets the property identifier.
            /// </summary>
            /// <param name="propertyName">Name of the JSON property.</param>
            protected abstract string GetPropertyId(string propertyName);
        }
    }
}
