using SMod3.Core.API;
using SMod3.Module.Commands.Meta;

namespace SMod3.Module.Commands.Console
{
	/// <summary>
	///		The main command handler for the console commands.
	/// </summary>
	public abstract class BaseConsoleCommandHandler : BaseCommandHandler
	{
		/// <summary>
		///		Color of the returned response in the console.
		///		<para>
		///			Set the value each time the response is returned.
		///		</para>
		/// </summary>
		public abstract ColorType ResponseColor { get; set; }
	}
}
