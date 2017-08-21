using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Conker.Spec
{
    [Binding]
    public class CommandParsingSteps
    {
        public abstract class CommandHandler
        {
            private readonly Action _callback;

            protected CommandHandler(Action callback)
            {
                _callback = callback;
            }

            public IReadOnlyList<object> CapturedArguments { get; private set; }

            protected void Invoke(object arg)
            {
                CapturedArguments = new[] {arg};
                _callback();
            }

            protected void Invoke(object arg1, object arg2)
            {
                CapturedArguments = new[] {arg1, arg2};
                _callback();
            }
        }

        private readonly Dictionary<string, (MethodInfo method, object target)> _commands 
            = new Dictionary<string, (MethodInfo method, object target)>();

        private string _invokedCommandName;

        private CommandHandler _commandHandler;
        
        private static readonly Regex Whitespace = new Regex("\\s+");

        [Given(@"I have a handler for the command ""(.*)"" which doesn't take arguments")]
        public void GivenIHaveAHandlerForTheCommandWhichDoesnTTakeArguments(string commandName)
        {
            void Command() => _invokedCommandName = commandName;

            Action cmd = Command;

            _commands.Add(commandName, (cmd.Method, cmd.Target));
        }
        
        [When(@"I run my application with the args ""(.*)""")]
        public void WhenIRunMyApplicationWithTheArgs(string args)
        {
            var values = Whitespace.Split(args);

            var handler = _commands[values[0]];

            var parameters = new object[values.Length - 1];

            Array.Copy(values, 1, parameters, 0, values.Length - 1);

            handler.method.Invoke(handler.target, parameters);
        }
        
        [Then(@"the ""(.*)"" command is invoked")]
        public void ThenTheCommandIsInvoked(string commandName)
        {
            _invokedCommandName.Should().Be(commandName);
        }

        [Given(@"I have a handler for the command ""(.*)"" which takes the following arguments")]
        public void GivenIHaveAHandlerForTheCommandWhichTakesTheFollowingArguments(string commandName, Table table)
        {
            var parameters = table.Rows.Select(row => new
                {
                    Name = row["name"],
                    Type = row["type"]
                })
                .Select(p => new
                {
                    p.Name,
                    FullTypeName = p.Type.Contains(Type.Delimiter) ? p.Type : "System." + p.Type
                })
                .Select(p => new
                {
                    p.Name,
                    Type = Type.GetType(p.FullTypeName, true)
                })
                .ToArray();

            var parameterTypes = parameters.Select(p => p.Type).ToArray();

            var assemblyName = new AssemblyName("TestCommandHandlers");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            var typeBuilder = moduleBuilder.DefineType(
                "DynamicCommandHandler",
                TypeAttributes.Public | TypeAttributes.Class,
                typeof(CommandHandler));

            var constructorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new[] {typeof(Action)});

            var constructorBodyGenerator = constructorBuilder.GetILGenerator();

            var baseCtor = typeof(CommandHandler).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Single();
            constructorBodyGenerator.Emit(OpCodes.Ldarg_0);
            constructorBodyGenerator.Emit(OpCodes.Ldarg_1);
            constructorBodyGenerator.Emit(OpCodes.Call, baseCtor);
            constructorBodyGenerator.Emit(OpCodes.Ret);

            // Create the handler method with the type parameters specified.
            var handler = typeBuilder.DefineMethod(
                "Handle",
                MethodAttributes.Public,
                CallingConventions.Standard,
                typeof(void),
                parameterTypes);

            var handlerBodyGenerator = handler.GetILGenerator();
            
            // Load the "this" argument onto the stack.
            handlerBodyGenerator.Emit(OpCodes.Ldarg_0);

            // Load all declared arguments onto the stack.
            for (var i = 0; i < parameters.Length; i++)
            {
                handlerBodyGenerator.Emit(OpCodes.Ldarg, i+1);
            }

            // Call the protected "Invoke" method to save the arguments.
            var captureMethod = typeof(CommandHandler)
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(method => method.Name == "Invoke")
                .Single(method => method.GetParameters().Length == parameters.Length);
            
            handlerBodyGenerator.EmitCall(OpCodes.Call, captureMethod, null);

            // Return from the method.
            handlerBodyGenerator.Emit(OpCodes.Ret);

            var handlerType = typeBuilder.CreateType();

            void Command() => _invokedCommandName = commandName;

            _commandHandler = (CommandHandler)Activator.CreateInstance(handlerType, (Action)Command);

            var handlerMethod = handlerType.GetMethod("Handle");

            _commands.Add(commandName, (handlerMethod, _commandHandler));
        }

        [Then(@"the ""(.*)"" command is invoked with the following arguments")]
        public void ThenTheCommandIsInvokedWithTheFollowingArguments(string commandName, Table table)
        {
            _invokedCommandName.Should().Be(commandName);

            var arguments = table.Rows.Select(row => new
            {
                Name = row["name"],
                Type = row["type"],
                Value = row["value"]
            });

            _commandHandler.CapturedArguments.Should().BeEquivalentTo(arguments.Select(arg => arg.Value));
        }

    }
}
