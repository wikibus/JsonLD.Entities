using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NullGuard;

namespace JsonLD.Entities.Context
{
    /// <summary>
    /// JSON-LD extensions to <see cref="JProperty" />
    /// </summary>
    [NullGuard(ValidationFlags.All)]
    public static class PropertyExtensions
    {
        private static readonly JsonLdContractResolver ContractResolver = new JsonLdContractResolver();

        /// <summary>
        /// Gets the property from LINQ expression.
        /// </summary>
        /// <typeparam name="T">target type</typeparam>
        /// <typeparam name="TReturn">The property return type.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <exception cref="System.ArgumentException">Parameter must be a property access expression</exception>
        public static PropertyInfo GetProperty<T, TReturn>(this Expression<Func<T, TReturn>> propertyExpression)
        {
             if (!(propertyExpression.Body is MemberExpression))
            {
                throw new ArgumentException("Parameter must be a property access expression", nameof(propertyExpression));
            }

            var memberExpression = (MemberExpression)propertyExpression.Body;
            return (PropertyInfo)memberExpression.Member;
        }

        /// <summary>
        /// Gets the name of the JSON key the <paramref name="property"/>
        /// will be serialized to.
        /// </summary>
        public static string GetJsonPropertyName(this PropertyInfo property)
        {
            var jsonProperty = property.GetCustomAttributes(typeof(JsonPropertyAttribute), true)
                                       .Cast<JsonPropertyAttribute>().SingleOrDefault();
            if (jsonProperty != null && jsonProperty.PropertyName != null)
            {
                return jsonProperty.PropertyName;
            }

            return ContractResolver.GetResolvedPropertyName(property.Name);
        }

        /// <summary>
        /// Ensures the property is an expanded definition.
        /// </summary>
        internal static JProperty EnsureExpandedDefinition(this JProperty property)
        {
            if (property.Value is JObject)
            {
                return property;
            }

            return new JProperty(property.Name, new JObject(new JProperty(JsonLdKeywords.Id, property.Value)));
        }

        /// <summary>
        /// Appends a property to the property definition
        /// </summary>
        internal static JProperty With(this JProperty property, string name, [AllowNull] string value)
        {
            property.Value[name] = value;
            return property;
        }
    }
}
