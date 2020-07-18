using System.Collections.Generic;

using SMod3.API;
using SMod3.Module.EventSystem.Background;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

namespace SMod3.Module.EventSystem.Events
{
    public sealed class DecideRespawnQueueEvent : EventArg
    {
        public Dictionary<Player, TeamType> Players { get; }

        public DecideRespawnQueueEvent()
        {
            Players = new Dictionary<Player, TeamType>();
        }

        internal override void Reset()
        {
            Players.Clear();
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<DecideRespawnQueueEvent>(other);
            foreach (var pair in Players)
                target.Players.Add(pair.Key, pair.Value);
        }
    }

    public sealed class TeamRespawnEvent : EventArg
    {
        public List<Player> Players { get; }
        public SpawnableTeamType Team { get; set; }

        public TeamRespawnEvent()
        {
            Players = null;
        }

        internal override void Reset()
        {
            Players.Clear();
            Team = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<TeamRespawnEvent>(other);
            target.Players.AddRange(Players);
            target.Team = Team;
        }
    }
}
