using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NullGuard;

namespace JsonLD.Entities.Converters
{
    /// <summary>
    /// Converts objects to JSON-LD
    /// </summary>
    public sealed class JsonLdConverter : JsonConverter
    {
        private readonly IContextProvider _contextProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLdConverter"/> class.
        /// </summary>
        /// <param name="contextProvider">The @context provider.</param>
        public JsonLdConverter(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, [AllowNull] object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            var context = _contextProvider.GetContext(value.GetType());

            if (context != null && IsNotEmpty(context))
            {
                writer.WritePropertyName("@context");
                serializer.Serialize(writer, context);
            }

            var types = GetTypes(value.GetType());
            if (types.Any())
            {
                writer.WritePropertyName("@type");
                writer.WriteStartArray();
                foreach (var type in types)
                {
                    writer.WriteValue(type);
                }

                writer.WriteEndArray();
            }

            var objectContract = (JsonObjectContract)serializer.ContractResolver.ResolveContract(value.GetType());
            foreach (var property in objectContract.Properties)
            {
                var propVal = property.ValueProvider.GetValue(value);
                if (propVal != null)
                {
                    writer.WritePropertyName(property.PropertyName);
                    serializer.Serialize(writer, propVal);
                }
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, [AllowNull] object existingValue, JsonSerializer serializer)
        {
            var instance = Activator.CreateInstance(objectType);

            serializer.Populate(reader, instance);

            return instance;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        ///   <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        private static JArray GetTypes(Type modelType)
        {
            var classes =
                from attr in modelType.GetCustomAttributes(typeof(ClassAttribute), false).OfType<ClassAttribute>()
                let classUri = attr.ClassUri
                select new JValue(classUri);

            return new JArray(classes.Cast<object>().ToArray());
        }

        private static bool IsNotEmpty(JToken context)
        {
            if (context is JObject)
            {
                return ((JObject)context).Count > 0;
            }

            if (context is JArray)
            {
                return context.All(IsNotEmpty);
            }

            return false;
        }
    }
}
