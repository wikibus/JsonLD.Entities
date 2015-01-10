using System;
using NullGuard;

namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// A literal node
    /// </summary>
    public class Literal : Node
    {
        private readonly string _value;
        private readonly Uri _datatype;
        private readonly string _languageTag;

        /// <summary>
        /// Initializes a new instance of the <see cref="Literal"/> class.
        /// </summary>
        public Literal(string value, Uri datatype)
            : this(value)
        {
            _datatype = datatype;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Literal"/> class.
        /// </summary>
        public Literal(string value, string languageTag)
            : this(value)
        {
            _languageTag = languageTag;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Literal"/> class.
        /// </summary>
        public Literal(string value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public string Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Gets the datatype.
        /// </summary>
        public Uri Datatype
        {
            [return: AllowNull]
            get { return _datatype; }
        }

        /// <summary>
        /// Gets the language tag.
        /// </summary>
        public string LanguageTag
        {
            [return: AllowNull]
            get { return _languageTag; }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            string format = "\"{0}\"";

            if (_datatype != null)
            {
                format += "^^<{1}>";
            }
            else if (_languageTag != null)
            {
                format += "@{2}";
            }

            return string.Format(format, _value, _datatype, _languageTag);
        }
    }
}
