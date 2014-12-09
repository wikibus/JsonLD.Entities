using System.Collections.Generic;

namespace JsonLD.Entities.Tests.Entities
{
    public class HasInterestsList
    {
        public IList<string> Interests { get; set; }

        public void AddInterest(string interst)
        {
            Interests.Add(interst);
        }
    }
}
