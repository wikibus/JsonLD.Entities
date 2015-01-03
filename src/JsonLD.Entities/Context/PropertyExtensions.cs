using Newtonsoft.Json.Linq;
using NullGuard;

namespace JsonLD.Entities.Context
{
    /// <summary>
    /// JSON-LD extensions to <see cref="JProperty" />
    /// </summary>
    [NullGuard(ValidationFlags.All)]
    internal static class PropertyExtensions
    {
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
