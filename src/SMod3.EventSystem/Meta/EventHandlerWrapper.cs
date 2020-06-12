using SMod3.Core;
using SMod3.Core.Fundamental;
using SMod3.Module.EventSystem.EventHandlers.Meta;

namespace SMod3.Module.EventSystem.Meta
{
    public sealed class EventHandlerWrapper : BaseWrapper
    {
        public Priority Priority { get; }

        public IEventHandler Handler { get; }

        public EventHandlerWrapper(Plugin owner, Priority priority, IEventHandler handler) : base(owner)
        {
            Priority = priority;
            Handler = handler;
        }
    }
}
