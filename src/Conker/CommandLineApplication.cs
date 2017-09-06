using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Conker
{
    public class CommandLineApplication
    {
        private readonly Dictionary<string, (MethodInfo method, object target)> _commands
            = new Dictionary<string, (MethodInfo method, object target)>();

        private (MethodInfo method, object target)? _handler;

        public string Name { get; set; } = System.Diagnostics.Process.GetCurrentProcess().ProcessName;

        public TextWriter OutputWriter { get; set; } = Console.Out;

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
            if (!args.Any() || args.Count == 1 && args[0] == "--help")
            {
                PrintHelp();
                return;
            }

            if (args.Count > 1 && args[args.Count - 1] == "--help")
            {
                PrintHelp(args[0], _commands[args[0]]);
                return;
            }

            if (_handler != null)
            {
                InvokeHandler(_handler.Value, args);
            }
            else
            {
                InvokeHandler(_commands[args[0]], args, 1);
            }
        }

        private void PrintHelp()
        {
            var usage = new StringBuilder();

            usage.Append("usage: ");
            usage.Append(Name);

            if (_commands.Any())
            {
                usage.AppendLine(" <command> [<args>]");
                usage.AppendLine();

                usage.AppendLine("The following commands are available:");

                foreach (var command in _commands)
                {
                    usage.Append("   ");
                    usage.AppendLine(command.Key);
                }
            }
            else
            {
                foreach (var parameter in _handler.Value.method.GetParameters())
                {
                    usage.Append(" <");
                    usage.Append(parameter.Name);
                    usage.AppendLine(">");
                }
            }

            OutputWriter.Write(usage);
            OutputWriter.WriteLine();
            OutputWriter.Flush();
        }

        private void PrintHelp(string command, (MethodInfo method, object target) handler)
        {
            var parameters = handler.method.GetParameters();

            var usage = new StringBuilder();

            usage.Append("usage: ");
            usage.Append(Name);
            usage.Append(" ");
            usage.Append(command);

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

        private void InvokeHandler((MethodInfo method, object target) handler, IReadOnlyList<string> args, int index = 0)
        {
            // Convert argument strings to expected types.
            var parameters = handler.method.GetParameters();
            var arguments = new object[parameters.Length];

            var lastArgIndex = (args.Count - index) - 1;

            for (var i = 0; i < parameters.Length; i++)
            {
                if (i > lastArgIndex)
                {
                    OutputWriter.Write("error: missing parameter '");
                    OutputWriter.Write(parameters[i].Name);
                    OutputWriter.WriteLine("'");
                    OutputWriter.WriteLine();
                    OutputWriter.Flush();
                    return;
                }

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