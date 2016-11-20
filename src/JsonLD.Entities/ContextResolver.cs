using System;
using System.Linq;
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
        private readonly IContextProvider contextProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextResolver"/> class.
        /// </summary>
        /// <param name="contextProvider">The context provider.</param>
        public ContextResolver(IContextProvider contextProvider)
        {
            this.contextProvider = contextProvider;
        }

        /// <summary>
        /// Gets @context for an entity type.
        /// </summary>
        [return: AllowNull]
        public JToken GetContext(Type type)
        {
            var context = this.GetContextFromProvider(type) ?? GetContextFromProperty(type);

            return EnsureContextType(context);
        }

        /// <summary>
        /// Gets @context for an entity instance.
        /// </summary>
        [return: AllowNull]
        public JToken GetContext(object entity)
        {
            var context = this.GetContextFromProvider(entity.GetType())
                          ?? GetContextFromMethod(entity)
                          ?? GetContextFromProperty(entity.GetType());

            return EnsureContextType(context);
        }

        private static JToken EnsureContextType(dynamic context)
        {
            if (context == null)
            {
                return null;
            }

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

        private static dynamic GetContextFromProperty(Type type)
        {
            try
            {
                const string propertyName = "get_Context";
                InvokeMemberName invokeArgs = propertyName;
                if (type.IsGenericTypeDefinition)
                {
                    var typeArgs = Enumerable.Repeat(typeof(object), type.GetGenericArguments().Length);
                    type = type.MakeGenericType(typeArgs.ToArray());
                }

                return Impromptu.InvokeGet(type.WithStaticContext(), "Context");
            }
            catch (RuntimeBinderException)
            {
                return null;
            }
        }

        private static dynamic GetContextFromMethod(object entity)
        {
            try
            {
                return Impromptu.InvokeMember(entity.GetType().WithStaticContext(), "GetContext", entity);
            }
            catch (RuntimeBinderException)
            {
                return null;
            }
        }

        private JToken GetContextFromProvider(Type type)
        {
            var context = this.contextProvider.GetContext(type);

            if (context != null)
            {
                return context;
            }

            return null;
        }
    }
}
