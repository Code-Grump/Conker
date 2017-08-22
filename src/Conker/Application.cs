using System;
using System.Collections.Generic;
using System.Reflection;

namespace Conker
{
    public class Application
    {
        private readonly Dictionary<string, (MethodInfo method, object target)> _commands
            = new Dictionary<string, (MethodInfo method, object target)>();

        public void AddCommand(string commandName, Action cmd)
        {
            _commands.Add(commandName, (cmd.Method, cmd.Target));
        }

        public void AddCommand(string commandName, MethodInfo handlerMethod, object target)
        {
            _commands.Add(commandName, (handlerMethod, target));
        }

        public void Execute(IReadOnlyList<string> args)
        {
            var handler = _commands[args[0]];

            var arguments = new object[args.Count - 1];

            // Convert argument strings to expected types.
            var parameters = handler.method.GetParameters();

            for (var i = 0; i < parameters.Length; i++)
            {
                arguments[i] = Convert.ChangeType(args[i + 1], parameters[i].ParameterType);
            }

            handler.method.Invoke(handler.target, arguments);
        }
    }
}