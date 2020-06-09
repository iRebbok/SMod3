using SMod3.Module.EventSystem.EventHandlers.Meta;
using SMod3.Module.EventSystem.Events.Meta;

namespace SMod3.Module.EventSystem.Events
{
    public class SetConfigEvent : Event
    {
        public string Key { get; }
        public object Value { get; set; }
        public object DefaultValue { get; }

        public SetConfigEvent(string key, object value, object def)
        {
            Key = key;
            Value = value;
            DefaultValue = def;
        }

        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerSetConfig)handler).OnSetConfig(this);
        }
    }
}
