using SMod3.Module.EventSystem.Background;
using SMod3.Module.EventSystem.Events;

namespace SMod3.Module.EventSystem.Handlers
{
    public interface IEventHandlerRoundStart : IEventHandler
    {
        void OnRoundStart();
    }

    public interface IEventHandlerRoundEnd : IEventHandler
    {
        void OnRoundEnd(RoundEndEvent ev);
    }

    public interface IEventHandlerCheckRoundEnd : IEventHandler
    {
        void OnCheckRoundEnd(CheckRoundEndEvent ev);
    }

    public interface IEventHandlerWaitingForPlayers : IEventHandler
    {
        void OnWaitingForPlayers();
    }

    public interface IEventHandlerRoundRestart : IEventHandler
    {
        void OnRoundRestart();
    }

    public interface IEventHandlerSetServerName : IEventHandler
    {
        void OnSetServerName(SetServerNameEvent ev);
    }

    /// <summary>
    ///     Called on any game event except
    ///     <see cref="IEventHandlerUpdate"/>,
    ///     <see cref="IEventHandlerFixedUpdate"/>
    ///     and <see cref="IEventHandlerLateUpdate"/>.
    /// </summary>
    public interface IEventHandlerGenericEvent : IEventHandler
    {
        void OnGenericEvent(EventArg? ev);
    }

    public interface IEventHandlerUpdate : IEventHandler
    {
        void OnUpdate();
    }

    public interface IEventHandlerFixedUpdate : IEventHandler
    {
        void OnFixedUpdate();
    }

    public interface IEventHandlerLateUpdate : IEventHandler
    {
        void OnLateUpdate();
    }

    public interface IEventHandlerSceneChanged : IEventHandler
    {
        void OnSceneChanged(SceneChangedEvent ev);
    }

    public interface IEventHandlerSetSeed : IEventHandler
    {
        void OnSetSeed(SetSeedEvent ev);
    }
}

