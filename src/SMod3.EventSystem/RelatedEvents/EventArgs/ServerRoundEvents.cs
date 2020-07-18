
using SMod3.API;
using SMod3.Module.EventSystem.Background;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

namespace SMod3.Module.EventSystem.Events
{
    public sealed class RoundEndEvent : EventArg
    {
        public LeadingTeam LeadingTeam { get; set; }
        public ROUND_END_STATUS Status { get; internal set; }

        internal override void Reset()
        {
            LeadingTeam = default;
            Status = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<RoundEndEvent>(other);
            target.LeadingTeam = LeadingTeam;
            target.Status = Status;
        }
    }

    public sealed class ConnectEvent : EventArg
    {
        public Connection Connection { get; internal set; }

        internal override void Reset()
        {
            Connection = null;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<ConnectEvent>(other);
            target.Connection = Connection;
        }
    }

    public sealed class CheckRoundEndEvent : EventArg
    {
        public ROUND_END_STATUS Status { get; set; }

        internal override void Reset()
        {
            Status = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<CheckRoundEndEvent>(other);
            target.Status = Status;
        }
    }

    public sealed class SetServerNameEvent : EventArg
    {
        public string Name { get; set; }

        internal override void Reset()
        {
            Name = null;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<SetServerNameEvent>(other);
            target.Name = Name;
        }
    }

    public sealed class SceneChangedEvent : EventArg
    {
        public string SceneName { get; internal set; }

        internal override void Reset()
        {
            SceneName = null;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<SceneChangedEvent>(other);
            target.SceneName = SceneName;
        }
    }

    public sealed class SetSeedEvent : EventArg
    {
        public int Seed { get; set; }

        internal override void Reset()
        {
            Seed = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<SetSeedEvent>(other);
            target.Seed = Seed;
        }
    }
}
