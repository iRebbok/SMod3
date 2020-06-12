using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using SMod3.API;
using SMod3.Core;

namespace SMod3.Module.Command.Meta
{
    /// <summary>
    ///		Static class for handling internal calls from the command manager.
    /// </summary>
    public static class UniversalMethods
    {
        public static Regex ArgumentRegex { get; } = new Regex("\"(.+)\"|\\s*'(.+)'|([^\\s]+)", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        ///		Processes a command call using a query.
        /// </summary>
        public static string[]? CallCommand<T>(Dictionary<string, CommandWrapper<T>> commandWrappers, ICommandSender sender, string query)
                where T : BaseCommandHandler
        {
            //PluginManager.Manager.Logger.Debug(nameof(UniversalMethods.CallCommand), $"Attempt to call a command using a query: {query}");

            // A regex may throw an exception, and we'll process it
            string[]? output = null;
            try
            {
                if (TryGetWrapper(commandWrappers, query, out CommandWrapper<T> wrapper, out string[] args))
                {
                    output = CallCommand(sender, wrapper.Handler, args);
                }
            }
            catch (Exception e)
            {
                //PluginManager.Manager.Logger.Error(nameof(UniversalMethods.CallCommand), $"Somehow went wrong: {e.InnerException} {e.Message}");

                //PluginManager.Manager.Logger.Debug(nameof(UniversalMethods.CallCommand), $"Query: {query}");
                //PluginManager.Manager.Logger.Debug(nameof(UniversalMethods.CallCommand), e.StackTrace);
            }
            return output;
        }
        /// <summary>
        ///		Checks whether the wrapper needed for further processing exists.
        /// </summary>
        /// <param name="wrappers">
        ///		HashSet containing wrappers for searching.
        /// </param>
        /// <param name="wrapper">
        ///		The appropriate wrapper.
        /// </param>
        public static bool TryGetWrapper<T>(Dictionary<string, CommandWrapper<T>> commandWrappers, string query, out CommandWrapper<T> wrapper, out string[] args)
                where T : BaseCommandHandler
        {
            wrapper = null;
            args = null;

            string command = query.Substring(0, query.IndexOf(' '));
            var regexMatch = ArgumentRegex.Match(query.Substring(command.Length));
            if (regexMatch != null && regexMatch.Success)
            {
                List<string> arguments = new List<string>();
                foreach (Group group in regexMatch.Groups)
                {
                    if (group.Success && !string.IsNullOrEmpty(group.Value))
                    {
                        arguments.Add(group.Value);
                    }
                }
                args = arguments.ToArray();
            }
            if (commandWrappers.TryGetValue(command, out var commandWrapper)) wrapper = commandWrapper;
            return wrapper != null;
        }
        /// <summary>
        ///		Processes a command call using a handler.
        /// </summary>
        public static string[] CallCommand<T>(ICommandSender sender, T handler, params string[] args)
                where T : BaseCommandHandler
        {
            //PluginManager.Manager.Logger.Debug(nameof(UniversalMethods.CallCommand),
            //    $"Calling command {handler.GetType().FullName} with args: {string.Join(", ", args?.Length == 0 ? new string[] { "No args." } : args)}");

            string[] output;
            try
            {
                output = handler.OnCall(sender, args);
            }
            catch (Exception ex)
            {
                output = new string[]
                {
                    "Command failed to excute and threw an exception:",
                    $"ExSource: {ex.Source}",
                    $"ExMessage: {ex.Message}",
                    $"ExStackTrace:\n{ex.StackTrace}"
                };
            }
            return output;
        }
        /// <summary>
        ///		Universal method for registering a command in a wrapper.
        /// </summary>
        public static bool RegisterCommand<T>(Dictionary<string, CommandWrapper<T>> commandWrappers, Dictionary<Plugin, HashSet<CommandWrapper<T>>> pluginWrappers,
            Plugin owner, string command, T handler)
                where T : BaseCommandHandler
        {
            if (IsRepeated(commandWrappers, command)) return false;

            if (!TryGetWrapper(pluginWrappers, owner, handler, out CommandWrapper<T> wrapper))
            {
                wrapper = new CommandWrapper<T>(owner, handler, new[] { command });
            }

            commandWrappers.Add(command, wrapper);
            wrapper.Commands.Add(command);

            return true;
        }

        public static bool IsRepeated<T>(Dictionary<string, CommandWrapper<T>> commandWrappers, string command)
                where T : BaseCommandHandler
        {
            return commandWrappers.ContainsKey(command);
        }
        /// <summary>
        ///		Checks whether the wrapper needed for further processing exists.
        /// </summary>
        /// <param name="wrappers">
        ///		HashSet containing wrappers for searching.
        /// </param>
        /// <param name="wrapper">
        ///		The appropriate wrapper.
        /// </param>
        public static bool TryGetWrapper<T>(Dictionary<Plugin, HashSet<CommandWrapper<T>>> pluginWrappers, Plugin owner, T handler, out CommandWrapper<T> wrapper)
                where T : BaseCommandHandler
        {
            wrapper = null;
            if (pluginWrappers.TryGetValue(owner, out var wrappers)) wrapper = wrappers.FirstOrDefault(iWrapper => ReferenceEquals(iWrapper.Handler, handler));
            else pluginWrappers.Add(owner, new HashSet<CommandWrapper<T>>());
            return wrapper != null;
        }
        /// <summary>
        ///		Destroys the command.
        /// </summary>
        public static void Dispose<T>(Dictionary<string, CommandWrapper<T>> commandWrappers, Dictionary<Plugin, HashSet<CommandWrapper<T>>> pluginWrappers,
            string command, bool fullyWrapper = false)
                where T : BaseCommandHandler
        {
            if (!commandWrappers.TryGetValue(command, out var wrapper)) return;

            if (fullyWrapper)
            {
                foreach (var cmd in wrapper.Commands) commandWrappers.Remove(cmd);
                pluginWrappers[wrapper.Owner].Remove(wrapper);
            }
            else
            {
                wrapper.Commands.Remove(command);
                commandWrappers.Remove(command);
            }
        }
        /// <summary>
        ///		Destroys resources used by the plugin.
        /// </summary>
        public static void Dispose<T>(Dictionary<string, CommandWrapper<T>> commandWrappers, Dictionary<Plugin, HashSet<CommandWrapper<T>>> pluginWrappers, Plugin owner)
                where T : BaseCommandHandler
        {
            if (!pluginWrappers.TryGetValue(owner, out var wrappers)) return;

            foreach (var wrapper in wrappers) foreach (var command in wrapper.Commands)
                {
                    commandWrappers.Remove(command);
                }

            pluginWrappers.Remove(owner);
        }
        /// <summary>
        ///		Destroys resources used by the hanlder.
        /// </summary>
        public static void Dispose<T>(Dictionary<string, CommandWrapper<T>> commandWrappers, Dictionary<Plugin, HashSet<CommandWrapper<T>>> pluginWrappers, T handler)
                where T : BaseCommandHandler
        {
            var pluginwrappers = pluginWrappers.Where(pluginWrapper => pluginWrapper.Value.Any(wrapper => ReferenceEquals(wrapper.Handler, handler))).ToList();
            foreach (var pluginWrapper in pluginwrappers) foreach (var iWrapper in pluginWrapper.Value)
                {
                    foreach (var command in iWrapper.Commands) commandWrappers.Remove(command);
                    pluginWrappers.Remove(pluginWrapper.Key);
                }
        }
    }
}
