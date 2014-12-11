using System;
using System.Collections;
using System.Globalization;
using ImpromptuInterface;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace JsonLD.Entities.Tests.Bindings
{
    [Binding]
    public class EntityVerificationSteps
    {
        private static object Entity
        {
            get { return ScenarioContext.Current["Entity"]; }
        }

        [Then(@"object should have property '(.*)' equal to '(.*)'")]
        public void ThenObjectShouldHavePropertyEqualTo(string propertyName, string expectedValue)
        {
            var actualValue = Impromptu.InvokeGet(Entity, propertyName);

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        [Then(@"object should have DateTime property '(.*)' equal to '(\d\d-\d\d-\d\d\d\d)'")]
        public void ThenObjectShouldHaveDateTimePropertyEqualTo(string propertyName, string expectedDateString)
        {
            var actualValue = Impromptu.InvokeGet(Entity, propertyName);
            var expectedValue = DateTime.ParseExact(expectedDateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }

        [Then(@"object should have property '(.*)' containg string '(.*)'")]
        public void ThenObjectShouldHavePropertyContaingString(string propName, string expectedValue)
        {
            IEnumerable collection = Impromptu.InvokeGet(Entity, propName);

            Assert.That(collection, Contains.Item(expectedValue));
        }

        [Then(@"object should have object property '(.*)'")]
        public void ThenObjectShouldHaveObjectProperty(string propertyName)
        {
            var actualValue = Impromptu.InvokeGet(Entity, propertyName);
            Assert.That(actualValue, Is.Not.Null);
            ScenarioContext.Current.Set(actualValue, propertyName);
        }

        [Then(@"object '(.*)' should have property '(.*)' equal to '(.*)'")]
        public void ThenObjectShouldHavePropertyEqualTo(string objectName, string propertyName, string expectedValue)
        {
            var obj = ScenarioContext.Current[objectName];
            var actualValue = Impromptu.InvokeGet(obj, propertyName);

            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }
    }
}
