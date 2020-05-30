using System;

namespace SMod3.Module.Commands.Meta
{
	/// <summary>
	///		A generic type for defining custom command handlers
	///		for each command system (the remote admin and console).
	///		<para>
	///			Don't use it to define commands.
	///		</para>
	/// </summary>
	public abstract class BaseCommandHandler
	{
		/// <summary>
		///		Method that is called when the command is triggered.
		/// </summary>
		/// <param name="sender">
		///		Sender of the command.
		///		<para>
		///			This can be a player or server, use
		///			<code>
		///				if (sender is Smod2.API.Player player)
		///				{
		///					/* your stuff */
		///				}
		///			</code>
		///			to verify the sender.
		///		</para>
		/// </param>
		/// <param name="args">
		///		Command args.
		///		<para>
		///			Note: args can be null, always check for null.
		///			You can use null-conditional operators to get access via null args avoiding <see cref="NullReferenceException"/>.
		///		</para>
		/// </param>
		public abstract string[] OnCall(ICommandSender sender, params string[] args);
	}
}
