using SMod3.Module.EventSystem.EventHandlers.Meta;

namespace SMod3.Module.EventSystem.Events.Meta
{
	public abstract class Event
	{
		public abstract void ExecuteHandler(IEventHandler handler);
	}
}
