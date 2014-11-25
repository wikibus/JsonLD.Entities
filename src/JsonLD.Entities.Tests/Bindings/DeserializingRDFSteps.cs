using TechTalk.SpecFlow;

namespace JsonLD.Entities.Tests.Bindings
{
    [Binding]
    public class DeserializingRDFSteps
    {
        private readonly DeserializationContext _context;

        public DeserializingRDFSteps(DeserializationContext context)
        {
            _context = context;
        }

        [Given(@"NQuads:")]
        public void GivenRDFData(string nQuads)
        {
            _context.NQuads = nQuads;
        }
    }
}