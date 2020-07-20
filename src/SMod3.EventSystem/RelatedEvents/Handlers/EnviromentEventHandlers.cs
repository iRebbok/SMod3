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

    public interface IEventHandlerWarheadStartCountdown : IEventHandler
    {
        void OnWarheadStartCountdown(WarheadStartCountdownEvent ev);
    }

    public interface IEventHandlerWarheadStopCountdown : IEventHandler
    {
        void OnWarheadStopCountdown(WarheadStopCountdownEvent ev);
    }

    public interface IEventHandlerWarheadChangeLever : IEventHandler
    {
        void OnWarheadChangeLever(WarheadChangeLeverEvent ev);
    }

    public interface IEventHandlerWarheadKeycardAccess : IEventHandler
    {
        void OnWarheadKeycardAccess(WarheadKeycardAccessEvent ev);
    }

    public interface IEventHandlerWarheadDetonate : IEventHandler
    {
        void OnWarheadDetonate();
    }

    public interface IEventHandlerLCZDecontaminate : IEventHandler
    {
        void OnLCZDecontaminate();
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
