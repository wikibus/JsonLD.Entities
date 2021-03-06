﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.4.0.0
//      SpecFlow Generator Version:2.4.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace JsonLD.Entities.Tests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class SerializingFeature : Xunit.IClassFixture<SerializingFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "Serializing.feature"
#line hidden
        
        public SerializingFeature(SerializingFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Serializing", "    Test serializing models to JSON-LD", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute(DisplayName="Serialize simple model with blank id")]
        [Xunit.TraitAttribute("FeatureTitle", "Serializing")]
        [Xunit.TraitAttribute("Description", "Serialize simple model with blank id")]
        public virtual void SerializeSimpleModelWithBlankId()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Serialize simple model with blank id", null, ((string[])(null)));
#line 4
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 5
    testRunner.Given("a person without id", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
    testRunner.When("the object is serialized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 7
    testRunner.Then("the resulting JSON-LD should be:", "{\r\n    \"name\": \"Tomasz\",\r\n    \"surname\": \"Pluskiewicz\",\r\n    \"birthDate\": \"1972-0" +
                    "9-04T00:00:00\",\r\n    \"age\": 30\r\n}", ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute(DisplayName="Serialize model with single element in set")]
        [Xunit.TraitAttribute("FeatureTitle", "Serializing")]
        [Xunit.TraitAttribute("Description", "Serialize model with single element in set")]
        [Xunit.InlineDataAttribute("JsonLD.Entities.Tests.Entities.HasInterestsArray", new string[0])]
        [Xunit.InlineDataAttribute("JsonLD.Entities.Tests.Entities.HasInterestsSet", new string[0])]
        [Xunit.InlineDataAttribute("JsonLD.Entities.Tests.Entities.HasInterestsCollection", new string[0])]
        [Xunit.InlineDataAttribute("JsonLD.Entities.Tests.Entities.HasInterestsEnumerable", new string[0])]
        [Xunit.InlineDataAttribute("JsonLD.Entities.Tests.Entities.HasInterestsGenerator", new string[0])]
        public virtual void SerializeModelWithSingleElementInSet(string type, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Serialize model with single element in set", null, exampleTags);
#line 17
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 18
    testRunner.Given(string.Format("model of type \'{0}\'", type), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 19
      testRunner.And("model has interest \'RDF\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
     testRunner.When("the object is serialized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 21
     testRunner.Then("the resulting JSON-LD should be:", "{\r\n    \"interests\": [ \"RDF\" ]\r\n}", ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="Serialize model with single element in list")]
        [Xunit.TraitAttribute("FeatureTitle", "Serializing")]
        [Xunit.TraitAttribute("Description", "Serialize model with single element in list")]
        public virtual void SerializeModelWithSingleElementInList()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Serialize model with single element in list", null, ((string[])(null)));
#line 35
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 36
    testRunner.Given("model of type \'JsonLD.Entities.Tests.Entities.HasInterestsList\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 37
      testRunner.And("model has interest \'RDF\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 38
     testRunner.When("the object is serialized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 39
     testRunner.Then("the resulting JSON-LD should be:", "{\r\n    \"interests\": [ \"RDF\" ]\r\n}", ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.TheoryAttribute(DisplayName="Serialize model with empty collection")]
        [Xunit.TraitAttribute("FeatureTitle", "Serializing")]
        [Xunit.TraitAttribute("Description", "Serialize model with empty collection")]
        [Xunit.InlineDataAttribute("JsonLD.Entities.Tests.Entities.HasInterestsGenerator", new string[0])]
        [Xunit.InlineDataAttribute("JsonLD.Entities.Tests.Entities.HasInterestsSet", new string[0])]
        public virtual void SerializeModelWithEmptyCollection(string type, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Serialize model with empty collection", null, exampleTags);
#line 46
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 47
    testRunner.Given(string.Format("model of type \'{0}\'", type), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 48
     testRunner.When("the object is serialized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 49
     testRunner.Then("the resulting JSON-LD should be:", "{\r\n}", ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="Serialize model with prefixed name in ClassAttribute")]
        [Xunit.TraitAttribute("FeatureTitle", "Serializing")]
        [Xunit.TraitAttribute("Description", "Serialize model with prefixed name in ClassAttribute")]
        public virtual void SerializeModelWithPrefixedNameInClassAttribute()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Serialize model with prefixed name in ClassAttribute", null, ((string[])(null)));
#line 59
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 60
    testRunner.Given("model of type \'JsonLD.Entities.Tests.Entities.PersonWithPrefixedClass\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 61
    testRunner.And("@context is:", "{\r\n   \"ex\": \"http://example.com/ontology#\"\r\n}", ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
    testRunner.When("the object is serialized", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 68
    testRunner.Then("the resulting JSON-LD should be:", "{\r\n    \"@type\": \"ex:Person\"\r\n}", ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                SerializingFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                SerializingFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
