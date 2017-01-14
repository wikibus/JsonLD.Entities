using System;
using System.Xml;
using Newtonsoft.Json;
using NullGuard;

namespace JsonLD.Entities.Converters
{
    /// <summary>
    /// Converts .NET framework data types as expanded literals typed with XSD data types accordingly
    /// </summary>
    public class BuiltInTypesLiteralConverter : JsonLdLiteralConverter
    {
        /// <summary>
        /// Implicitly typed JS values are serialized as compact literals.
        /// </summary>
        protected override bool ShouldSerializeAsCompactLiteral(object value)
        {
            return value?.GetType().HasImplicitlyTypedJsonType() == true;
        }

        /// <summary>
        /// Writes the RDF string representation of XSD types to JSON.
        /// </summary>
        protected override void WriteJsonLdValue(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value?.GetType().HasImplicitlyTypedJsonType() == true)
            {
                writer.WriteValue(value);
            }
            else if (value is TimeSpan)
            {
                writer.WriteValue(XmlConvert.ToString((TimeSpan)value));
            }
            else
            {
                var valueString = JsonConvert.ToString(value);
                writer.WriteValue(valueString.Trim('"')); // for the weirdest of reasons, DateTime is double-quoted
            }
        }

        /// <summary>
        /// Gets the XSD data type URI matching the value.
        /// </summary>
        protected override string GetJsonLdType([AllowNull] object value)
        {
            var type = value?.GetType();
            return type.GetXsdType();
        }
    }
}