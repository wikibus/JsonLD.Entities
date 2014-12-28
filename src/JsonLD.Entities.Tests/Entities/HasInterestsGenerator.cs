using System.Collections.Generic;

namespace JsonLD.Entities.Tests.Entities
{
    public class HasInterestsGenerator
    {
        private readonly IList<string> _interests;

        public HasInterestsGenerator()
        {
            _interests = new List<string>();
        }

        public IEnumerable<string> Interests
        {
            get
            {
                foreach (var interest in _interests)
                {
                    yield return interest;
                }
            }
        }

        public void AddInterest(string interst)
        {
            _interests.Add(interst);
        }
    }
}
