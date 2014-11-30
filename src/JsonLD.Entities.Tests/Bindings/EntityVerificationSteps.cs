using System;
using System.Globalization;
using ImpromptuInterface;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace JsonLD.Entities.Tests.Bindings
{
    [Binding]
    public class EntityVerificationSteps
    {
        [Then(@"object should have property '(.*)' equal to '(.*)'")]
        public void ThenObjectShouldHavePropertyEqualTo(string propertyName, string expectedValue)
        {
            var entity = ScenarioContext.Current["Entity"];
            var actualValue = Impromptu.InvokeGet(entity, propertyName);

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        [Then(@"object should have DateTime property '(.*)' equal to '(\d\d-\d\d-\d\d\d\d)'")]
        public void ThenObjectShouldHaveDateTimePropertyEqualTo(string propertyName, string expectedDateString)
        {
            var entity = ScenarioContext.Current["Entity"];
            var actualValue = Impromptu.InvokeGet(entity, propertyName);
            var expectedValue = DateTime.ParseExact(expectedDateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }
    }
}
