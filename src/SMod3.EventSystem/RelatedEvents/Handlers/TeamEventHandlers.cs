using SMod3.Module.EventSystem.Background;
using SMod3.Module.EventSystem.Events;

namespace SMod3.Module.EventSystem.Handlers
{
    public interface IEventHandlerDecideTeamRespawnQueue : IEventHandler
    {
        void OnDecideTeamRespawnQueue(DecideRespawnQueueEvent ev);
    }

    public interface IEventHandlerTeamRespawn : IEventHandler
    {
        void OnTeamRespawn(TeamRespawnEvent ev);
    }
}
