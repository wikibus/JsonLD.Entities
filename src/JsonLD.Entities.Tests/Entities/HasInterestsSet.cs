using System.Collections.Generic;

namespace JsonLD.Entities.Tests.Entities
{
    public class HasInterestsSet
    {
        public HasInterestsSet()
        {
            Interests = new HashSet<string>();
        }

        public ISet<string> Interests { get; set; }

        public void AddInterest(string interst)
        {
            Interests.Add(interst);
        }
    }
}
