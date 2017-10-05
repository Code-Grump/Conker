﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.2.0.0
//      SpecFlow Generator Version:2.2.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Conker.Spec
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.2.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Verbs")]
    public partial class VerbsFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Verbs.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Verbs", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Invoke a verb without arguments")]
        public virtual void InvokeAVerbWithoutArguments()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Invoke a verb without arguments", ((string[])(null)));
#line 3
this.ScenarioSetup(scenarioInfo);
#line 4
 testRunner.Given("I have a handler for the verb \"test\" which doesn\'t take arguments", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 5
 testRunner.When("I run my application with the args \"test\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 6
 testRunner.Then("the \"test\" handler is invoked", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Invoke a verb with a string argument")]
        public virtual void InvokeAVerbWithAStringArgument()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Invoke a verb with a string argument", ((string[])(null)));
#line 8
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "type"});
            table1.AddRow(new string[] {
                        "name",
                        "String"});
#line 9
 testRunner.Given("I have a handler for the verb \"test\" which requires the following arguments", ((string)(null)), table1, "Given ");
#line 12
 testRunner.When("I run my application with the args \"test FooBar\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "type",
                        "value"});
            table2.AddRow(new string[] {
                        "name",
                        "String",
                        "FooBar"});
#line 13
 testRunner.Then("the \"test\" handler is invoked with the following arguments", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Invoke a verb with an integer argument")]
        public virtual void InvokeAVerbWithAnIntegerArgument()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Invoke a verb with an integer argument", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "type"});
            table3.AddRow(new string[] {
                        "size",
                        "Int32"});
#line 18
 testRunner.Given("I have a handler for the verb \"test\" which requires the following arguments", ((string)(null)), table3, "Given ");
#line 21
 testRunner.When("I run my application with the args \"test 12345\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "type",
                        "value"});
            table4.AddRow(new string[] {
                        "size",
                        "Int32",
                        "12345"});
#line 22
 testRunner.Then("the \"test\" handler is invoked with the following arguments", ((string)(null)), table4, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Invoke a verb with a decimal argument")]
        public virtual void InvokeAVerbWithADecimalArgument()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Invoke a verb with a decimal argument", ((string[])(null)));
#line 26
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "type"});
            table5.AddRow(new string[] {
                        "size",
                        "Double"});
#line 27
 testRunner.Given("I have a handler for the verb \"test\" which requires the following arguments", ((string)(null)), table5, "Given ");
#line 30
 testRunner.When("I run my application with the args \"test 12345.6789\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "type",
                        "value"});
            table6.AddRow(new string[] {
                        "size",
                        "Double",
                        "12345.6789"});
#line 31
 testRunner.Then("the \"test\" handler is invoked with the following arguments", ((string)(null)), table6, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Invoke a verb with too many arguments")]
        public virtual void InvokeAVerbWithTooManyArguments()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Invoke a verb with too many arguments", ((string[])(null)));
#line 35
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "type"});
            table7.AddRow(new string[] {
                        "size",
                        "Int32"});
#line 36
 testRunner.Given("I have a handler for the verb \"test\" which requires the following arguments", ((string)(null)), table7, "Given ");
#line 39
 testRunner.When("I run my application with the args \"test 12345 678910\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "type",
                        "value"});
            table8.AddRow(new string[] {
                        "size",
                        "Int32",
                        "12345"});
#line 40
 testRunner.Then("the \"test\" handler is invoked with the following arguments", ((string)(null)), table8, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Invoke a verb with too few arguments")]
        public virtual void InvokeAVerbWithTooFewArguments()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Invoke a verb with too few arguments", ((string[])(null)));
#line 44
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "name",
                        "type"});
            table9.AddRow(new string[] {
                        "size",
                        "Int32"});
            table9.AddRow(new string[] {
                        "count",
                        "Int32"});
#line 45
 testRunner.Given("I have a handler for the verb \"test\" which requires the following arguments", ((string)(null)), table9, "Given ");
#line 49
 testRunner.When("I run my application with the args \"test 12345\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 50
 testRunner.Then("the application prints the error \"missing required parameter \'count\'\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
