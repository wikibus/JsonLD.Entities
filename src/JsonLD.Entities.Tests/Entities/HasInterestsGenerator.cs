using System.Collections.Generic;

namespace JsonLD.Entities.Tests.Entities
{
    public class HasInterestsGenerator
    {
        private readonly IList<string> interests;

        public HasInterestsGenerator()
        {
            this.interests = new List<string>();
        }

        public IEnumerable<string> Interests
        {
            get
            {
                foreach (var interest in this.interests)
                {
                    yield return interest;
                }
            }
        }

        public void AddInterest(string interst)
        {
            this.interests.Add(interst);
        }
    }
}
