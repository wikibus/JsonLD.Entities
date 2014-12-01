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
            return JObject.Parse(@"{
                'foaf': 'http://xmlns.com/foaf/0.1/',
                'name': 'foaf:givenName',
                'lastName': 'foaf:familyName',
                'lastName': 'http://example.com/ontology#dateOfBirth'
            }");
        }
    }
}
