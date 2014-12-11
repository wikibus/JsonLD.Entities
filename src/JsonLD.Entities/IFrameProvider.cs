using System;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities
{
    /// <summary>
    /// Contract for classes, which provide a JSON-LD frame
    /// to be used for deserializing complex objects
    /// </summary>
    public interface IFrameProvider
    {
        /// <summary>
        /// Gets the frame.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <remarks>
        /// If the frame contains a @context, it will be replaced by
        /// a context provided by <see cref="IContextProvider"/>
        /// </remarks>
        JObject GetFrame(Type modelType);
    }
}
