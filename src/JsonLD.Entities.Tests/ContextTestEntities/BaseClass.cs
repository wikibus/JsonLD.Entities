using Newtonsoft.Json.Linq;

namespace JsonLD.Entities.Tests.ContextTestEntities
{
    public class BaseClass
    {
        protected static JObject GetContext(BaseClass obj)
        {
            return new JObject
            {
                { "foaf", "http://xmlns.com/foaf/0.1/" }
            };
        }
    }
}
