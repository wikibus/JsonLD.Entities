using System;

namespace JsonLD.Entities.Tests.Entities
{
    public class PersonWithPrefixedClass
    {
        public Uri Id { get; set; }

        public string Type
        {
            get { return "ex:Person"; }
        }
    }
}
