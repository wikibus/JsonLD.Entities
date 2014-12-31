using System;
using ImpromptuInterface;
using ImpromptuInterface.InvokeExt;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using NullGuard;

namespace JsonLD.Entities
{
    /// <summary>
    /// Resolves a @context for entity types
    /// </summary>
    public sealed class ContextResolver
    {
        private readonly IContextProvider _contextProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextResolver"/> class.
        /// </summary>
        /// <param name="contextProvider">The context provider.</param>
        public ContextResolver(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        /// <summary>
        /// Gets @context for an entity type.
        /// </summary>
        [return: AllowNull]
        public JToken GetContext(Type type)
        {
            dynamic context = _contextProvider.GetContext(type);

            if (context != null)
            {
                return context;
            }

            try
            {
                context = Impromptu.InvokeGet(type.WithStaticContext(), "Context");
            }
            catch (RuntimeBinderException)
            {
                return null;
            }

            return EnsureContextType(context);
        }

        /// <summary>
        /// Gets @context for an entity instance.
        /// </summary>
        [return: AllowNull]
        public JToken GetContext(object entity)
        {
            dynamic context = _contextProvider.GetContext(entity.GetType());

            if (context != null)
            {
                return context;
            }

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
