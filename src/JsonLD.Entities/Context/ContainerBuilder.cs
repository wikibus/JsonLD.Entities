using System;

namespace JsonLD.Entities.Context
{
    /// <summary>
    /// Used to define property's @container
    /// </summary>
    public class ContainerBuilder
    {
        /// <summary>
        /// The property is a @set container
        /// </summary>
        public PropertyBuilder Set()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The property is a @list container
        /// </summary>
        public PropertyBuilder List()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The property is a @index container
        /// </summary>
        public PropertyBuilder Index()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The property is a @language map
        /// </summary>
        public PropertyBuilder Language()
        {
            throw new NotImplementedException();
        }
    }
}
