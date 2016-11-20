using System;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities.Context
{
    /// <summary>
    /// Automatic @context based on properties and class identifier
    /// </summary>
    /// <typeparam name="T">Model type</typeparam>
    /// <seealso cref="Newtonsoft.Json.Linq.JObject" />
    public class AutoContext<T> : AutoContextBase<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoContext{T}"/> class.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        public AutoContext(Uri classId)
            : base(new ClassNameStrategy(classId))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoContext{T}"/> class
        /// by extending definitions from <paramref name="context"/>
        /// </summary>
        /// <param name="context">The current @context.</param>
        /// <param name="classId">The class identifier.</param>
        public AutoContext(JObject context, Uri classId)
            : base(context, new ClassNameStrategy(classId))
        {
        }

        private class ClassNameStrategy : AutoContextStrategy
        {
            private const string SlashClassIdAppendFormat = "{0}#{1}";
            private const string HashClassIdAppendFormat = "{0}/{1}";
            private readonly Uri classId;

            public ClassNameStrategy(Uri classId)
            {
                this.classId = classId;
            }

            protected override string GetPropertyId(string propertyName)
            {
                var format = HashClassIdAppendFormat;

                if (string.IsNullOrWhiteSpace(this.classId.Fragment))
                {
                    format = SlashClassIdAppendFormat;
                }

                return string.Format(format, this.classId, propertyName);
            }
        }
    }
}
