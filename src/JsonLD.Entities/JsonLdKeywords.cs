using System.Collections.Generic;
using NullGuard;

namespace JsonLD.Entities
{
    /// <summary>
    /// Contains JSON-LD special property names
    /// </summary>
    [NullGuard(ValidationFlags.All)]
    public static class JsonLdKeywords
    {
        /// <summary>
        /// @id keyword
        /// </summary>
        public const string Id = "@id";

        /// <summary>
        /// @type keyword
        /// </summary>
        public const string Type = "@type";

        /// <summary>
        /// @base keyword
        /// </summary>
        public const string Base = "@base";

        /// <summary>
        /// @vocab keyword
        /// </summary>
        public const string Vocab = "@vocab";

        /// <summary>
        /// @context keyword
        /// </summary>
        public const string Context = "@context";

        /// <summary>
        /// @language keyword
        /// </summary>
        public const string Language = "@language";

        /// <summary>
        /// @container keyword
        /// </summary>
        public const string Container = "@container";

        /// <summary>
        /// @list keyword
        /// </summary>
        public const string List = "@list";

        /// <summary>
        /// @set keyword
        /// </summary>
        public const string Set = "@set";

        /// <summary>
        /// @index keyword
        /// </summary>
        public const string Index = "@index";

        /// <summary>
        /// @value keyword
        /// </summary>
        public const string Value = "@value";

        private static readonly IDictionary<string, string> KnownPropertyNames = new Dictionary<string, string>();

        static JsonLdKeywords()
        {
            KnownPropertyNames.Add("Id", Id);
            KnownPropertyNames.Add("Type", Type);
            KnownPropertyNames.Add("Types", Type);
            KnownPropertyNames.Add("Context", Context);
        }

        /// <summary>
        /// Determines whether the specified value is a JSON-LD keyword.
        /// </summary>
        public static bool IsKeyword(string value)
        {
            return value == Id ||
                   value == Type ||
                   value == Base ||
                   value == Vocab ||
                   value == Context ||
                   value == Container ||
                   value == List ||
                   value == Set ||
                   value == Index;
        }

        /// <summary>
        /// Gets the keyword for a C# property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>a JSON-LD keyword or null</returns>
        [return: AllowNull]
        internal static string GetKeywordForProperty(string propertyName)
        {
            KnownPropertyNames.TryGetValue(propertyName, out propertyName);

            return propertyName;
        }
    }
}
