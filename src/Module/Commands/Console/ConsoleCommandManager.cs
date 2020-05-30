using SMod3.Module.Commands.Meta;

namespace SMod3.Module.Commands.Console
{
	public sealed class ConsoleCommandManager : BaseCommandManager<BaseConsoleCommandHandler>
	{
		public static ConsoleCommandManager Manager { get; } = new ConsoleCommandManager();

		private ConsoleCommandManager() { }

		public override string LoggingTag => "CONSOLE_COMMAND_MANAGER";
	}
}
