using System;
using Newtonsoft.Json.Linq;
using NullGuard;

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
        [return: AllowNull]
        public JObject GetFrame(Type modelType)
        {
            return null;
        }
    }
}
