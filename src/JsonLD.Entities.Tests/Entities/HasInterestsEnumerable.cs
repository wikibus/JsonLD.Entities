using System.Collections.Generic;
using System.Linq;

namespace JsonLD.Entities.Tests.Entities
{
    public class HasInterestsEnumerable
    {
        private IList<string> interests;

        public HasInterestsEnumerable()
        {
            this.Interests = new List<string>();
        }

        public IEnumerable<string> Interests
        {
            get { return this.interests; }
            set { this.interests = value.ToList(); }
        }

        public void AddInterest(string interst)
        {
            this.interests.Add(interst);
        }
    }
}
