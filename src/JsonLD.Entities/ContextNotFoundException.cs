using System;

namespace JsonLD.Entities
{
    /// <summary>
    /// Represents errors which occur if JSON-LD @context cannot be found for a given type
    /// </summary>
    public class ContextNotFoundException : Exception
    {
        private readonly Type _entityType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextNotFoundException"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        public ContextNotFoundException(Type entityType)
        {
            _entityType = entityType;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message
        {
            get { return string.Format("JSON-LD context not found for type {0}", _entityType.FullName); }
        }
    }
}
