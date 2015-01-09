using System;

namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// Represents errors that occur during parsing
    /// </summary>
    public class ParsingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingException"/> class.
        /// </summary>
        public ParsingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingException"/> class.
        /// </summary>
        public ParsingException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingException"/> class.
        /// </summary>
        public ParsingException(string message) : base(message)
        {
        }
    }
}
