using System;

namespace JsonLD.Entities.Parsing
{
    /// <summary>
    /// Data for event of parsing a quad
    /// </summary>
    public class QuadParsedEventArgs : EventArgs
    {
        private readonly Quad _quad;
        private readonly int _line;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuadParsedEventArgs"/> class.
        /// </summary>
        public QuadParsedEventArgs(Quad quad, int line)
        {
            _quad = quad;
            _line = line;
        }

        /// <summary>
        /// Gets the quad.
        /// </summary>
        public Quad Quad
        {
            get { return _quad; }
        }

        /// <summary>
        /// Gets the line.
        /// </summary>
        public int Line
        {
            get { return _line; }
        }
    }
}
