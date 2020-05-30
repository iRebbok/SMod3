using SMod3.Module.EventSystem.EventHandlers.Meta;
using SMod3.Module.EventSystem.Events;

namespace SMod3.Module.EventSystem
{
	public interface IEventHandlerSetConfig : IEventHandler
	{
		void OnSetConfig(SetConfigEvent ev);
	}
}

