using System;
using ImpromptuInterface;
using ImpromptuInterface.InvokeExt;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities
{
    /// <summary>
    /// Resolves a @context for entity types
    /// </summary>
    public sealed class ContextResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextResolver"/> class.
        /// </summary>
        /// <param name="contextProvider">The context provider.</param>
        public ContextResolver(IContextProvider contextProvider)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextResolver"/> class.
        /// </summary>
        public ContextResolver()
        {
        }

        /// <summary>
        /// Gets @context for an entity type.
        /// </summary>
        public JToken GetContext(Type type)
        {
            dynamic context;
            try
            {
                context = Impromptu.InvokeGet(type.WithStaticContext(), "Context");
            }
            catch (RuntimeBinderException)
            {
                return new JObject();
            }

            return EnsureContextType(context);
        }

        /// <summary>
        /// Gets @context for an entity instance.
        /// </summary>
        public JToken GetContext(object entity)
        {
            dynamic context;
            try
            {
                context = Impromptu.InvokeMember(entity.GetType().WithStaticContext(), "GetContext", entity);
            }
            catch (RuntimeBinderException)
            {
                return GetContext(entity.GetType());
            }

            return EnsureContextType(context);
        }

        private static JToken EnsureContextType(dynamic context)
        {
            if (context is string)
            {
                return JToken.Parse(context);
            }

            if (context is JToken)
            {
                return context;
            }

            throw new InvalidOperationException(string.Format("Invalid context type {0}. Must be string or JToken", context.GetType()));
        }
    }
}
