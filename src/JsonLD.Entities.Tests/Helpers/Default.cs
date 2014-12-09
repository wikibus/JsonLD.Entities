using System;
using Newtonsoft.Json.Linq;

namespace JsonLD.Entities.Tests.Helpers
{
    public class Default
    {
        private static readonly Lazy<JObject> LazyPersonContext = new Lazy<JObject>(CreatePersonContext);
        private static readonly Lazy<JObject> LazyHasInterestsContext = new Lazy<JObject>(CreateHasInterestsContext);
        private static readonly Lazy<JObject> LazyHasInterestsListContext = new Lazy<JObject>(CreateHasInterestsListContext);

        public static JObject PersonContext
        {
            get { return LazyPersonContext.Value; }
        }

        public static JObject HasInterestsSetContext
        {
            get { return LazyHasInterestsContext.Value; }
        }

        public static JObject HasInterestsListContext
        {
            get { return LazyHasInterestsListContext.Value; }
        }

        private static JObject CreatePersonContext()
        {
            return JObject.Parse(@"{
                'foaf': 'http://xmlns.com/foaf/0.1/',
                'name': 'foaf:givenName',
                'surname': 'foaf:familyName',
                'birthDate': 'http://example.com/ontology#dateOfBirth'
            }");
        }

        private static JObject CreateHasInterestsContext()
        {
            return JObject.Parse(@"{
                'foaf': 'http://xmlns.com/foaf/0.1/',
                'interests': { '@id': 'foaf:topic_interest', '@container': '@set' }
            }");
        }

        private static JObject CreateHasInterestsListContext()
        {
            return JObject.Parse(@"{
                'foaf': 'http://xmlns.com/foaf/0.1/',
                'interests': { '@id': 'foaf:topic_interest', '@container': '@list' }
            }");
        }
    }
}
