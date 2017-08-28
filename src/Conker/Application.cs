using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Conker
{
    public class Application
    {
        private readonly Dictionary<string, (MethodInfo method, object target)> _commands
            = new Dictionary<string, (MethodInfo method, object target)>();

        private (MethodInfo method, object target)? _handler;

        public string Name { get; set; }

        public TextWriter OutputWriter { get; set; }

        public void AddVerb(string verb, Action cmd)
        {
            _commands.Add(verb, (cmd.Method, cmd.Target));
        }

        public void AddVerb(string verb, MethodInfo handlerMethod, object target)
        {
            _commands.Add(verb, (handlerMethod, target));
        }

        public void Execute(IReadOnlyList<string> args)
        {
            if (_handler != null)
            {
                var helpRequested = args.Count == 1 && args[0] == "--help";

                if (helpRequested)
                {
                    var handler = _handler.Value;

                    var parameters = handler.method.GetParameters();

                    var usage = new StringBuilder();

                    usage.Append("usage: ");
                    usage.Append(Name);
                    
                    foreach (var parameter in parameters)
                    {
                        usage.Append(" <");
                        usage.Append(parameter.Name);
                        usage.Append(">");
                    }

                    OutputWriter.WriteLine(usage);
                    OutputWriter.WriteLine();
                    OutputWriter.Flush();
                }

                InvokeHandler(_handler.Value, args);
            }
            else
            {
                InvokeHandler(_commands[args[0]], args, 1, args.Count - 1);
            }
        }

        private void InvokeHandler((MethodInfo method, object target) handler, IReadOnlyList<string> args)
        {
            InvokeHandler(handler, args, 0, args.Count);
        }

        private void InvokeHandler((MethodInfo method, object target) handler, IReadOnlyList<string> args, int index, int count)
        {
            var arguments = new object[count];

            // Convert argument strings to expected types.
            var parameters = handler.method.GetParameters();

            for (var i = 0; i < parameters.Length; i++)
            {
                arguments[i] = Convert.ChangeType(args[i + index], parameters[i].ParameterType);
            }

            handler.method.Invoke(handler.target, arguments);
        }

        public void SetHandler(MethodInfo handlerMethod, object target)
        {
            _handler = (handlerMethod, target);
        }
    }
}