using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using NullGuard;

namespace JsonLD.Entities
{
    /// <summary>
    /// Provides a predefined frame for each model type.
    /// </summary>
    public class StaticFrameProvider : IFrameProvider
    {
        private readonly IDictionary<Type, JObject> frames;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticFrameProvider"/> class.
        /// </summary>
        public StaticFrameProvider()
        {
            this.frames = new Dictionary<Type, JObject>();
        }

        /// <summary>
        /// Gets the frame.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <remarks>
        /// If the frame contains a @context, it will be replaced by
        /// a context provided by <see cref="IContextProvider" />
        /// </remarks>
        [return: AllowNull]
        public JObject GetFrame(Type modelType)
        {
            JObject context;
            this.frames.TryGetValue(modelType, out context);
            return context;
        }

        /// <summary>
        /// Sets frame used by given <paramref name="type"/>.
        /// </summary>
        public void SetFrame(Type type, JObject context)
        {
            this.frames[type] = context;
        }
    }
}
