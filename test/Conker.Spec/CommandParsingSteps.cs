using System;
using System.Collections.Generic;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Conker.Spec
{
    [Binding]
    public class CommandParsingSteps
    {
        private readonly Dictionary<string, Action> _commands = new Dictionary<string, Action>();

        private readonly List<string> _invokedCommands = new List<string>();

        [Given(@"I have a command ""(.*)"" which doesn't take arguments")]
        public void GivenIHaveACommandWhichDoesnTTakeArguments(string commandName)
        {
            _commands.Add(commandName, () => _invokedCommands.Add(commandName));
        }
        
        [When(@"I invoke my application with the args ""(.*)""")]
        public void WhenIInvokeMyApplicationWithTheArgs(string args)
        {
            _commands[args]();
        }
        
        [Then(@"the ""(.*)"" command is invoked")]
        public void ThenTheCommandIsInvoked(string commandName)
        {
            _invokedCommands.Should().BeEquivalentTo(commandName);
        }
    }
}
