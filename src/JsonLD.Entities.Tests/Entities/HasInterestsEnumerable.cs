using System.Collections.Generic;
using System.Linq;

namespace JsonLD.Entities.Tests.Entities
{
    public class HasInterestsEnumerable
    {
        private IList<string> _interests;

        public HasInterestsEnumerable()
        {
            Interests = new List<string>();
        }

        public IEnumerable<string> Interests
        {
            get { return _interests; }
            set { _interests = value.ToList(); }
        }

        public void AddInterest(string interst)
        {
            _interests.Add(interst);
        }
    }
}
