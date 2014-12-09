using System.Collections.Generic;
using System.Linq;

namespace JsonLD.Entities.Tests.Entities
{
    public class HasInterestsArray
    {
        private IList<string> _interests;

        public HasInterestsArray()
        {
            _interests = new List<string>();
        }

        public string[] Interests
        {
            get { return _interests.ToArray(); }
            set { _interests = value; }
        }

        public void AddInterest(string interst)
        {
            _interests.Add(interst);
        }
    }
}
