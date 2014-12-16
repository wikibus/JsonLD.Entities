using System;
using System.Collections;
using System.Collections.Generic;
using JsonLD.Entities.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonLD.Entities
{
    /// <summary>
    /// Camel-case contract resolver with overrides for JSON-LD keywords
    /// </summary>
    public class JsonLdContractResolver : CamelCasePropertyNamesContractResolver
    {
        private static readonly ICollection<Type> SetTypes = new HashSet<Type>();
        private static readonly ICollection<Type> ListTypes = new HashSet<Type>();
        private readonly IContextProvider _contextProvider;

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
        /// <param name="contextProvider">The @context provider.</param>
        public JsonLdContractResolver(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        /// <summary>
        /// Resolves the contract for a given type.
        /// </summary>
        /// <param name="type">The type to resolve a contract for.</param>
        public override JsonContract ResolveContract(Type type)
        {
            var contract = base.ResolveContract(type);

            if (contract is JsonObjectContract)
            {
                contract.Converter = new JsonLdConverter(_contextProvider);
            }
            else if (contract is JsonArrayContract)
            {
                Type converterType;
                Type elementType;

                if (type.BaseType == typeof(Array))
                {
                    elementType = type.GetElementType();
                    converterType = typeof(JsonLdArrayConverter<>);
                }
                else if (IsListType(type))
                {
                    elementType = type.GetGenericArguments()[0];
                    converterType = typeof(JsonLdListConverter<>);
                }
                else
                {
                    elementType = type.GetGenericArguments()[0];
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
                contract.Converter = new JsonLdLiteralConverter();
            }

            return contract;
        }

        /// <summary>
        /// Resolves the name of the property.
        /// </summary>
        protected override string ResolvePropertyName(string propertyName)
        {
            if (propertyName == "Id")
            {
                return "@id";
            }

            var resolvePropertyName = base.ResolvePropertyName(propertyName);
            return resolvePropertyName;
        }

        private static bool IsListType(Type type)
        {
            return typeof(IList).IsAssignableFrom(type)
                || (type.IsGenericType && typeof(IList<>).IsAssignableFrom(type.GetGenericTypeDefinition()));
        }

        private static bool IsSetType(Type type)
        {
            return SetTypes.Contains(type)
                || (type.IsGenericType && SetTypes.Contains(type.GetGenericTypeDefinition()));
        }
    }
}
