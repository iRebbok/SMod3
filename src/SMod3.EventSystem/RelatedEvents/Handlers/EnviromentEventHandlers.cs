using SMod3.Module.EventSystem.Background;
using SMod3.Module.EventSystem.Events;

namespace SMod3.Module.EventSystem.Handlers
{
    /// <summary>
    ///     Called during SCP914 activation.
    /// </summary>
    public interface IEventHandlerSCP914Activate : IEventHandler
    {
        void OnSCP914Activate(SCP914ActivateEvent ev);
    }

    public interface IEventHandlerWarheadStartCountdown : IEventHandler //Before the countdown is started
    {
        void OnStartCountdown(WarheadStartEvent ev);
    }
    public interface IEventHandlerWarheadStopCountdown : IEventHandler //Before the countdown is stopped
    {
        void OnStopCountdown(WarheadStopEvent ev);
    }

    public interface IEventHandlerWarheadChangeLever : IEventHandler
    {
        void OnChangeLever(WarheadChangeLeverEvent ev);
    }

    public interface IEventHandlerWarheadKeycardAccess : IEventHandler
    {
        void OnWarheadKeycardAccess(WarheadKeycardAccessEvent ev);
    }

    public interface IEventHandlerWarheadDetonate : IEventHandler
    {
        void OnDetonate();
    }

    public interface IEventHandlerLCZDecontaminate : IEventHandler
    {
        void OnDecontaminate();
    }

    public interface IEventHandlerGeneratorFinish : IEventHandler
    {
        void OnGeneratorFinish(GeneratorFinishEvent ev);
    }

    public interface IEventHandlerScpDeathAnnouncement : IEventHandler
    {
        void OnScpDeathAnnouncement(ScpDeathAnnouncementEvent ev);
    }
}
