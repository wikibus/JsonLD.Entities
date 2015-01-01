using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JsonLD.Entities.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonLD.Entities
{
    /// <summary>
    /// Camel-case contract resolver with overrides for JSON-LD keywords
    /// </summary>
    public sealed class JsonLdContractResolver : CamelCasePropertyNamesContractResolver
    {
        private static readonly ICollection<Type> SetTypes = new HashSet<Type>();
        private static readonly ICollection<Type> ListTypes = new HashSet<Type>();

        static JsonLdContractResolver()
        {
            SetTypes.Add(typeof(ICollection<>));
            SetTypes.Add(typeof(IEnumerable));
            SetTypes.Add(typeof(ISet<>));
            ListTypes.Add(typeof(IList));
            ListTypes.Add(typeof(IList<>));
        }

        /// <summary>
        /// Resolves the contract for a given type.
        /// </summary>
        /// <param name="type">The type to resolve a contract for.</param>
        public override JsonContract ResolveContract(Type type)
        {
            var contract = base.ResolveContract(type);

            if (contract is JsonArrayContract)
            {
                Type converterType;
                Type elementType = ((JsonArrayContract)contract).CollectionItemType;

                if (type.BaseType == typeof(Array))
                {
                    converterType = typeof(JsonLdArrayConverter<>);
                }
                else if (IsListType(type))
                {
                    converterType = typeof(JsonLdListConverter<>);
                }
                else
                {
                    converterType = typeof(JsonLdSetConverter<>);
                }

                contract.Converter = (JsonConverter)Activator.CreateInstance(converterType.MakeGenericType(elementType));
            }
            else if (type == typeof(Uri))
            {
                contract.Converter = new StringUriConverter();
            }
            else if (contract is JsonPrimitiveContract)
            {
                if (contract.Converter is JsonLdLiteralConverter)
                {
                    contract.Converter = null;
                }
                else
                {
                    contract.Converter = new JsonLdLiteralConverter();
                }
            }

            return contract;
        }

        /// <summary>
        /// Resolves the name of the property.
        /// </summary>
        protected override string ResolvePropertyName(string propertyName)
        {
            var keyword = JsonLdKeywords.GetKeywordForProperty(propertyName);
            if (keyword != null)
            {
                return keyword;
            }

            return base.ResolvePropertyName(propertyName);
        }

        /// <summary>
        /// Ensures that empty collections aren't serialized
        /// </summary>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
            {
                property.ShouldSerialize = instance =>
                {
                    var collection = property.ValueProvider.GetValue(instance);
                    return collection != null && ((IEnumerable)collection).Cast<object>().Any();
                };
            }

            return property;
        }

        private static bool IsListType(Type type)
        {
            return typeof(IList).IsAssignableFrom(type)
                || (type.IsGenericType && typeof(IList<>).IsAssignableFrom(type.GetGenericTypeDefinition()));
        }
    }
}
