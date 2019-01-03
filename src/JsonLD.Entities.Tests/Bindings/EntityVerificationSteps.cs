using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using Dynamitey;
using TechTalk.SpecFlow;
using Xunit;

namespace JsonLD.Entities.Tests.Bindings
{
    [Binding]
    public class EntityVerificationSteps
    {
        private readonly ScenarioContext scenarioContext;

        public EntityVerificationSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        private object Entity
        {
            get { return scenarioContext["Entity"]; }
        }

        [Then(@"object should have property '(.*)' equal to '(.*)'")]
        public void ThenObjectShouldHavePropertyEqualTo(string propertyName, string expectedValue)
        {
            var actualValue = Dynamic.InvokeGet(Entity, propertyName);

            Assert.Equal(expectedValue, actualValue);
        }

        [Then(@"object should have DateTime property '(.*)' equal to '(\d\d-\d\d-\d\d\d\d)'")]
        public void ThenObjectShouldHaveDateTimePropertyEqualTo(string propertyName, string expectedDateString)
        {
            var actualValue = Dynamic.InvokeGet(Entity, propertyName);
            var expectedValue = DateTime.ParseExact(expectedDateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            Assert.Equal(expectedValue, actualValue);
        }

        [Then(@"object should have property '(.*)' containg string '(.*)'")]
        public void ThenObjectShouldHavePropertyContaingString(string propName, string expectedValue)
        {
            IEnumerable collection = Dynamic.InvokeGet(Entity, propName);

            Assert.Contains(expectedValue, collection.Cast<string>());
        }

        [Then(@"object should have empty property '(.*)'")]
        public void ThenObjectShouldHaveEmptyProperty(string propName)
        {
            IEnumerable collection = Dynamic.InvokeGet(Entity, propName);

            Assert.Empty(collection);
        }

        [Then(@"object should have object property '(.*)'")]
        public void ThenObjectShouldHaveObjectProperty(string propertyName)
        {
            var actualValue = Dynamic.InvokeGet(Entity, propertyName);
            Assert.NotNull(actualValue);
            scenarioContext.Set(actualValue, propertyName);
        }

        [Then(@"object '(.*)' should have property '(.*)' equal to '(.*)'")]
        public void ThenObjectShouldHavePropertyEqualTo(string objectName, string propertyName, string expectedValue)
        {
            var obj = scenarioContext[objectName];
            var actualValue = Dynamic.InvokeGet(obj, propertyName);

            Assert.Equal(expectedValue, actualValue);
        }
    }
}
