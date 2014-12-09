using System.Collections.Generic;

namespace JsonLD.Entities.Tests.Entities
{
    public class HasInterestsCollection
    {
        public HasInterestsCollection()
        {
            Interests = new List<string>();
        }

        public ICollection<string> Interests { get; set; }

        public void AddInterest(string interst)
        {
            Interests.Add(interst);
        }
    }
}
