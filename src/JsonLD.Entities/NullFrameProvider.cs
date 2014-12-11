using System;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities
{
    /// <summary>
    /// Null-pattern for <see cref="IFrameProvider"/>
    /// </summary>
    internal class NullFrameProvider : IFrameProvider
    {
        /// <summary>
        /// Always return null
        /// </summary>
        public JObject GetFrame(Type modelType)
        {
            return null;
        }
    }
}
