using System;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities.Tests.Helpers
{
    public class Default
    {
        private static readonly Lazy<JObject> LazyPersonContext = new Lazy<JObject>(CreatePersonContext);

        public static JObject PersonContext
        {
            get { return LazyPersonContext.Value; }
        }

        private static JObject CreatePersonContext()
        {
            return JObject.Parse("{}");
        }
    }
}
