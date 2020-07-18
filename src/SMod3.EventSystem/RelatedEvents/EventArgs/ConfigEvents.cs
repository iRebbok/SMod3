using SMod3.Module.EventSystem.Background;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

namespace SMod3.Module.EventSystem.Events
{
    public sealed class SetConfigEvent : EventArg
    {
        public string Key { get; internal set; }
        public object Value { get; set; }
        public object DefaultValue { get; internal set; }

        internal override void Reset()
        {
            Key = null;
            Value = null;
            DefaultValue = null;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<SetConfigEvent>(other);
            target.Key = Key;
            target.Value = Value;
            target.DefaultValue = DefaultValue;
        }
    }
}
