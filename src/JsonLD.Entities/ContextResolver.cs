using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dynamitey;
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
                if (type.GetTypeInfo().IsGenericTypeDefinition)
                {
                    var typeArgs = Enumerable.Repeat(typeof(object), type.GetGenericArguments().Length);
                    type = type.MakeGenericType(typeArgs.ToArray());
                }

                var staticContext = InvokeContext.CreateStatic;
                return Dynamic.InvokeGet(staticContext(type), "Context");
            }
            catch (RuntimeBinderException)
            {
                return FallbackResolver.GetContext(type, null);
            }
        }

        private static dynamic GetContextFromMethod(object entity)
        {
            try
            {
                var staticContext = InvokeContext.CreateStatic;
                return Dynamic.InvokeMember(staticContext(entity.GetType()), "GetContext", entity);
            }
            catch (RuntimeBinderException)
            {
                return FallbackResolver.GetContext(entity.GetType(), entity);
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

        private static class FallbackResolver
        {
            private static readonly IDictionary<Type, Func<object, object>> ContextCache
                = new ConcurrentDictionary<Type, Func<object, object>>();

            public static dynamic GetContext(Type type, [AllowNull] object instance)
            {
                if (!ContextCache.ContainsKey(type))
                {
                    var getContextMethod = type
                        .GetMethod("GetContext", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

                    if (getContextMethod != null)
                    {
                        ContextCache.Add(type, e => getContextMethod.Invoke(null, new[] { e }));
                    }
                    else
                    {
                        var getContext = type
                            .GetProperty("Context", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

                        if (getContext != null)
                        {
                            ContextCache.Add(type, _ => getContext.GetMethod.Invoke(null, null));
                        }
                        else
                        {
                            ContextCache.Add(type, _ => null);
                        }
                    }
                }

                return ContextCache[type].Invoke(instance);
            }
        }
    }
}
