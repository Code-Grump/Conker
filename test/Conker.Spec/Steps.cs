using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Conker.Spec
{
    [Binding]
    public class Steps : TechTalk.SpecFlow.Steps
    {
        public class Argument
        {
            public Argument(string name, object value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; }

            public object Value { get; }

            public override bool Equals(object obj)
            {
                return obj is Argument argument &&
                       Name == argument.Name &&
                       EqualityComparer<object>.Default.Equals(Value, argument.Value);
            }

            public override int GetHashCode()
            {
                var hashCode = -244751520;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
                hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Value);
                return hashCode;
            }

            public override string ToString() => $"Name={Name}, Value={Value}";
        }

        public abstract class Handler
        {
            private readonly Action _callback;

            protected Handler(Action callback)
            {
                _callback = callback;
            }

            public bool Invoked { get; private set; }

            public IReadOnlyList<Argument> CapturedArguments { get; private set; }

            protected void Invoke(Argument arg)
            {
                CapturedArguments = new[] {arg};
                Invoked = true;
                _callback();
            }

            protected void Invoke(Argument arg1, Argument arg2)
            {
                CapturedArguments = new[] {arg1, arg2};
                Invoked = true;
                _callback();
            }
        }

        private string _invokedVerb;

        private Handler _handler;
        
        private static readonly Regex Whitespace = new Regex("\\s+");
        private readonly CommandLineApplication _application;
        private readonly StringWriter _outputWriter = new StringWriter();

        public Steps()
        {
            _application = new CommandLineApplication
            {
                OutputWriter = _outputWriter
            };
        }

        [Given(@"my application is called ""(.*)""")]
        public void GivenMyApplicationIsCalled(string name)
        {
            _application.Name = name;
        }

        [Given(@"I have an application ""(.*)"" which requires the following arguments")]
        public void GivenIHaveAnApplicationWhichRequiresTheFollowingArguments(string name, Table table)
        {
            GivenMyApplicationIsCalled(name);
            GivenIHaveAnApplicationWhichRequiresTheFollowingArguments(table);
        }

        [Given(@"I have an application which requires the following arguments")]
        public void GivenIHaveAnApplicationWhichRequiresTheFollowingArguments(Table table)
        {
            var parameters = GetParameters(table).ToArray();

            var handler = CreateHandler(parameters);

            _handler = handler.instance;

            _application.SetHandler(handler.method, handler.instance);
        }

        [Then(@"the application handler is invoked with the values")]
        public void ThenTheApplicationHandlerIsInvokedWithTheValues(Table table)
        {
            _handler.Invoked.Should().BeTrue(because: "the application handler should have been invoked");

            var expectedArguments = GetExpectedArguments(table);

            _handler.CapturedArguments.Should().BeEquivalentTo(expectedArguments);
        }

        [Given(@"I have a handler for the verb ""(.*)"" which doesn't take arguments")]
        public void GivenIHaveAHandlerForTheCommandWhichDoesnTTakeArguments(string commandName)
        {
            void Command() => _invokedVerb = commandName;

            Action cmd = Command;

            _application.AddVerb(commandName, cmd);
        }

        [When(@"I run my application with the args ""(.*)""")]
        public void WhenIRunMyApplicationWithTheArgs(string args)
        {
            var values = Whitespace.Split(args);

            _application.Execute(values);
        }

        [Then(@"the ""(.*)"" handler is invoked")]
        public void ThenTheCommandIsInvoked(string commandName)
        {
            _invokedVerb.Should().Be(commandName);
        }

        [Given(@"I have a handler for the verb ""(.*)"" which requires the following arguments")]
        public void GivenIHaveAHandlerForTheVerbWhichRequiresTheFollowingArguments(string commandName, Table table)
        {
            var parameters = GetParameters(table).ToArray();

            var handler = CreateHandler(parameters, () => _invokedVerb = commandName);

            _handler = handler.instance;

            _application.AddVerb(commandName, handler.method, _handler);
        }

        private static (Handler instance, MethodInfo method) CreateHandler(IReadOnlyList<(string name, Type type)> parameters) 
            => CreateHandler(parameters, () => {});

        private static (Handler instance, MethodInfo method) CreateHandler(IReadOnlyList<(string name, Type type)> parameters, Action callback)
        {
            var assemblyName = new AssemblyName("TestCommandHandlers");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            var typeBuilder = moduleBuilder.DefineType(
                "DynamicCommandHandler",
                TypeAttributes.Public | TypeAttributes.Class,
                typeof(Handler));

            var constructorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new[] {typeof(Action)});

            var constructorBodyGenerator = constructorBuilder.GetILGenerator();

            var baseCtor = typeof(Handler).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .Single();
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
                parameters.Select(p => p.type).ToArray());

            for (var i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];

                handler.DefineParameter(i + 1, ParameterAttributes.None, parameter.name);
            }

            var argumentConstructor = typeof(Argument).GetConstructors().Single();

            var handlerBodyGenerator = handler.GetILGenerator();

            // Load the "this" argument onto the stack.
            handlerBodyGenerator.Emit(OpCodes.Ldarg_0);

            // Load all declared arguments onto the stack as Argument values.
            for (var i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];

                handlerBodyGenerator.Emit(OpCodes.Ldstr, parameter.name);
                handlerBodyGenerator.Emit(OpCodes.Ldarg, i + 1);

                if (!parameter.type.IsClass)
                {
                    // Box the argument.
                    handlerBodyGenerator.Emit(OpCodes.Box, parameter.type);
                }

                handlerBodyGenerator.Emit(OpCodes.Newobj, argumentConstructor);
            }

            // Call the protected "Invoke" method to save the arguments.
            var captureMethod = typeof(Handler)
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(method => method.Name == "Invoke")
                .Single(method => method.GetParameters().Length == parameters.Count);

            handlerBodyGenerator.EmitCall(OpCodes.Call, captureMethod, null);

            // Return from the method.
            handlerBodyGenerator.Emit(OpCodes.Ret);

            var handlerType = typeBuilder.CreateType();

            var commandHandler = (Handler) Activator.CreateInstance(handlerType, callback);

            var handlerMethod = handlerType.GetMethod("Handle");

            return (commandHandler, handlerMethod);
        }

        private static IEnumerable<(string name, Type type)> GetParameters(Table table)
        {
            return table.Rows.Select(row => new
                {
                    Name = row["name"],
                    Type = row["type"]
                })
                .Select(p => new
                {
                    p.Name,
                    FullTypeName = p.Type.Contains(Type.Delimiter) ? p.Type : "System." + p.Type
                })
                .Select(p => (p.Name, Type.GetType(p.FullTypeName, true)));
        }

        [Then(@"the application prints usage information ""(.*)"" with the following commands")]
        public void ThenTheApplicationPrintsUsageInformationWithTheFollowingCommands(string expectedUsage, Table table)
        {
            var commands = table.Rows.Select(row => new {Name = row["name"]});

            var availableCommands = string.Join("", commands.Select(cmd => $"   {cmd.Name}\r\n"));

            var formattedUsage 
                = $"usage: {expectedUsage}\r\n"
                + "\r\n"
                + "The following commands are available:\r\n"
                + $"{availableCommands}\r\n";

            _outputWriter.ToString().Should().Be(formattedUsage);
        }

        [Then(@"the ""(.*)"" handler is invoked with the following arguments")]
        public void ThenTheHandlerIsInvokedWithTheFollowingArguments(string commandName, Table table)
        {
            _invokedVerb.Should().Be(commandName);

            var expectedArguments = GetExpectedArguments(table);

            _handler.CapturedArguments.Should().BeEquivalentTo(expectedArguments);
        }

        [When(@"I run my application with no args")]
        public void WhenIRunMyApplicationWithNoArgs()
        {
            _application.Execute(new string[0]);
        }

        [Then(@"the application prints usage information ""(.*)""")]
        public void ThenTheApplicationPrintsUsageInformation(string expectedUsage)
        {
            _outputWriter.ToString().Should().Be("usage: " + expectedUsage + "\r\n\r\n");
        }

        private static IEnumerable<Argument> GetExpectedArguments(Table table)
        {
            return table.Rows
                .Select(row => new
                {
                    Name = row["name"],
                    Type = row["type"],
                    Value = row["value"]
                })
                .Select(p => new
                {
                    p.Name,
                    FullTypeName = p.Type.Contains(Type.Delimiter) ? p.Type : "System." + p.Type,
                    p.Value
                })
                .Select(p => new
                {
                    p.Name,
                    Type = Type.GetType(p.FullTypeName, true),
                    Value = (object)p.Value
                })
                .Select(p => new Argument(p.Name, Convert.ChangeType(p.Value, p.Type)));
        }

        [Then(@"the application prints the error ""(.*)""")]
        public void ThenTheApplicationPrintsTheError(string message)
        {
            _outputWriter.ToString().Should().Be($"error: {message}\r\n\r\n");
        }
    }
}
