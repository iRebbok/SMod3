using System.Collections.Generic;

using SMod3.API;
using SMod3.Core;
using SMod3.Core.Meta;

namespace SMod3.Module.Command.Meta
{
    /// <summary>
    ///		A generic type for defining custom command managers
    ///		for each command system (remoteadmin and console).
    ///		<para>
    ///			Don't use it to define commands.
    ///		</para>
    /// </summary>
    /// <typeparam name="T">
    ///		Type of command handler.
    /// </typeparam>
    public abstract class BaseCommandManager<T> : BaseManager where T : BaseCommandHandler
    {
        /// <summary>
        ///		Dictionary of all commands that point to the command wrapper.
        /// </summary>
        public Dictionary<string, CommandWrapper<T>> CommandWrappers { get; } = new Dictionary<string, CommandWrapper<T>>();
        /// <summary>
        ///		Dictionary of all plugins that point to the command wrapper.
        /// </summary>
        public Dictionary<Plugin, HashSet<CommandWrapper<T>>> PluginWrappers { get; } = new Dictionary<Plugin, HashSet<CommandWrapper<T>>>();

        /// <summary>
        ///		Registers a native command for a subsequent trigger on the sender's side.
        /// </summary>
        /// <returns>
        ///		True if the command was successfully registered and false if something went wrong.
        /// </returns>
        public virtual bool RegisterCommand(Plugin owner, string command, T handler)
        {
            return UniversalMethods.RegisterCommand(CommandWrappers, PluginWrappers, owner, command, handler);
        }
        /// <summary>
        ///		Dispose the specific command.
        /// </summary>
        /// <returns>
        ///		Number of deleted records.
        ///		<para>
        ///			It should not exceed 1, because it allows register only one trigger per handler.
        ///		</para>
        /// </returns>
        public virtual void Dispose(string command, bool fullyWrapper = false)
        {
            UniversalMethods.Dispose(CommandWrappers, PluginWrappers, command, fullyWrapper);
        }
        /// <summary>
        ///		Dispose all commands registered by the plugin.
        /// </summary>
        /// <returns>
        ///		Number of deleted records.
        /// </returns>
        public override void Dispose(Plugin owner)
        {
            UniversalMethods.Dispose(CommandWrappers, PluginWrappers, owner);
        }
        /// <summary>
        ///		Dispose the specific handler.
        /// </summary>
        /// <returns>
        ///		Number of deleted records.
        ///		<para>
        ///			It should not exceed 1, because it allows register only one wrapper per handler.
        ///		</para>
        /// </returns>
        public virtual void Dispose(T handler)
        {
            UniversalMethods.Dispose(CommandWrappers, PluginWrappers, handler);
        }
        /// <summary>
        ///		Tries to call a command from query if it exists.
        /// </summary>
        /// <param name="sender">
        ///		Sender of the command.
        ///		<para>
        ///			Use <see cref="Server"/> to denote the sender of the plugin.
        ///		</para>
        /// </param>
        /// <param name="query">
        ///		Full query with arguments, command manager will get the arguments itself.
        /// </param>
        /// <returns>
        ///		Returns null if the command does not exist or
        ///		the plugin refuses to return the result,
        ///		the result of executing the command defined by the plugin.
        /// </returns>
        public virtual string[] CallCommand(ICommandSender sender, string query)
        {
            return UniversalMethods.CallCommand<T>(CommandWrappers, sender, query);
        }
        /// <summary>
        ///		Calls the specified command handler.
        /// </summary>
        /// <param name="sender">
        ///		Sender of the command.
        ///		<para>
        ///			Use <see cref="Server"/> to denote the sender of the plugin.
        ///		</para>
        /// </param>
        /// <param name="handler">
        ///		Handler of the command that will be called.
        /// </param>
        /// <param name="args">
        ///		Command arguments to pass to the handler.
        /// </param>
        /// <returns>
        ///		Returns null if the plugin refuses to return the result,
        ///		the result of executing the command defined by the plugin.
        /// </returns>
        public virtual string[] CallCommand(ICommandSender sender, T handler, params string[] args)
        {
            return UniversalMethods.CallCommand<T>(sender, handler, args);
        }
    }
}
