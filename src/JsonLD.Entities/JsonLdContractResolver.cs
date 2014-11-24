using System;
using Newtonsoft.Json.Serialization;

namespace JsonLD.Entities
{
    /// <summary>
    /// Camel-case contract resolver with overrides for JSON-LD keywords
    /// </summary>
    public class JsonLdContractResolver : CamelCasePropertyNamesContractResolver
    {
        private readonly IContextProvider _contextProvider;

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
    }
}
