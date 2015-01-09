using System;

namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// Data for event of parsing a triple
    /// </summary>
    public class TripleParsedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TripleParsedEventArgs"/> class.
        /// </summary>
        public TripleParsedEventArgs(Triple triple, int line)
        {
            Triple = triple;
            Line = line;
        }

        /// <summary>
        /// Gets the triple.
        /// </summary>
        public Triple Triple { get; private set; }

        /// <summary>
        /// Gets the line.
        /// </summary>
        public int Line { get; private set; }
    }
}
