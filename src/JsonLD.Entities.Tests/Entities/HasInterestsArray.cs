using System.Collections.Generic;
using System.Linq;

namespace JsonLD.Entities.Tests.Entities
{
    public class HasInterestsArray
    {
        private IList<string> interests;

        public HasInterestsArray()
        {
            this.interests = new List<string>();
        }

        public string[] Interests
        {
            get { return this.interests.ToArray(); }
            set { this.interests = value; }
        }

        public void AddInterest(string interst)
        {
            this.interests.Add(interst);
        }
    }
}
