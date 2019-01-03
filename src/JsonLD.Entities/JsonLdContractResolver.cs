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
    /// Camel-case contract resolver with overrides for JSON-LD features
    /// </summary>
    public sealed class JsonLdContractResolver : CamelCasePropertyNamesContractResolver
    {
        private static readonly ICollection<Type> SetTypes = new HashSet<Type>();
        private static readonly ICollection<Type> ListTypes = new HashSet<Type>();
        private static readonly JsonLdNamingStrategy JsonLdNamingStrategy = new JsonLdNamingStrategy();

        static JsonLdContractResolver()
        {
            SetTypes.Add(typeof(ICollection<>));
            SetTypes.Add(typeof(IEnumerable));
            SetTypes.Add(typeof(ISet<>));
            ListTypes.Add(typeof(IList));
            ListTypes.Add(typeof(IList<>));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLdContractResolver"/> class.
        /// </summary>
        public JsonLdContractResolver()
        {
            this.NamingStrategy = JsonLdNamingStrategy;
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

                if (type.GetTypeInfo().BaseType == typeof(Array))
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
            else if (contract is JsonPrimitiveContract && contract.Converter == null)
            {
                contract.Converter = new JsonLdLiteralConverter();
            }

            return contract;
        }

        /// <summary>
        /// Resolves the contract converter.
        /// </summary>
        protected override JsonConverter ResolveContractConverter(Type type)
        {
            if (type == typeof(Uri))
            {
                return new StringUriConverter();
            }

            return base.ResolveContractConverter(type);
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
                || (type.GetTypeInfo().IsGenericType && typeof(IList<>).IsAssignableFrom(type.GetGenericTypeDefinition()));
        }
    }
}
