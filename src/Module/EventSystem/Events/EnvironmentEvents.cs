using SMod3.Core.API;
using SMod3.Module.EventHandlers;
using SMod3.Module.EventSystem.EventHandlers.Meta;
using SMod3.Module.EventSystem.Events.Meta;

namespace SMod3.Module.EventSystem.Events
{
    public class SCP914ActivateEvent : Event
    {
        public Player User { get; }
        public KnobSetting KnobSetting { get; set; }
        public object[] Inputs { get; set; } //TODO: Proper wrapping API
        public Vector IntakePos { get; set; }
        public Vector OutputPos { get; set; }

        public SCP914ActivateEvent(Player user, KnobSetting knobSetting, object[] inputs, Vector intakePos, Vector outputPos)
        {
            User = user;
            KnobSetting = knobSetting;
            Inputs = inputs;
            IntakePos = intakePos;
            OutputPos = outputPos;
        }

        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerSCP914Activate)handler).OnSCP914Activate(this);
        }
    }

    public abstract class WarheadEvent : Event
    {
        public float TimeLeft { get; set; }
        public Player Activator { get; }
        public bool Cancel { get; set; }

        public WarheadEvent(Player player, float timeLeft)
        {
            Activator = player;
            TimeLeft = timeLeft;
            Cancel = false;
        }
    }

    public class WarheadStartEvent : WarheadEvent
    {
        public bool IsResumed { get; set; }
        public bool OpenDoorsAfter { get; set; }

        public WarheadStartEvent(Player activator, float timeLeft, bool isResumed, bool openDoorsAfter) : base(activator, timeLeft)
        {
            IsResumed = isResumed;
            OpenDoorsAfter = openDoorsAfter;
        }

        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerWarheadStartCountdown)handler).OnStartCountdown(this);
        }
    }

    public class WarheadStopEvent : WarheadEvent
    {
        public WarheadStopEvent(Player player, float timeLeft) : base(player, timeLeft)
        {
        }

        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerWarheadStopCountdown)handler).OnStopCountdown(this);
        }
    }

    public class WarheadChangeLeverEvent : Event
    {
        public Player Player { get; }
        public bool Allow { get; set; }

        public WarheadChangeLeverEvent(Player player)
        {
            Player = player;
            Allow = true;
        }

        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerWarheadChangeLever)handler).OnChangeLever(this);
        }
    }

    public class WarheadKeycardAccessEvent : Event
    {
        public Player Player { get; }
        public bool Allow { get; set; }

        public WarheadKeycardAccessEvent(Player player, bool allow)
        {
            Player = player;
            Allow = allow;
        }

        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerWarheadKeycardAccess)handler).OnWarheadKeycardAccess(this);
        }
    }

    public class WarheadDetonateEvent : Event
    {
        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerWarheadDetonate)handler).OnDetonate();
        }
    }

    public class LCZDecontaminateEvent : Event
    {
        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerLCZDecontaminate)handler).OnDecontaminate();
        }
    }

    public class SummonVehicleEvent : Event
    {
        public bool IsCI { get; set; }
        public bool AllowSummon { get; set; }

        public SummonVehicleEvent(bool isCI, bool allowSummon)
        {
            IsCI = isCI;
            AllowSummon = allowSummon;
        }

        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerSummonVehicle)handler).OnSummonVehicle(this);
        }
    }

    public class GeneratorFinishEvent : Event
    {
        public Generator Generator { get; }

        public GeneratorFinishEvent(Generator generator)
        {
            Generator = generator;
        }

        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerGeneratorFinish)handler).OnGeneratorFinish(this);
        }
    }

    public class ScpDeathAnnouncementEvent : Event
    {
        public bool ShouldPlay { get; set; }
        public Player DeadPlayer { get; }
        public RoleType PlayerRole { get; }

        public ScpDeathAnnouncementEvent(bool shouldPlay, Player deadPlayer, RoleType playerRole)
        {
            ShouldPlay = shouldPlay;
            DeadPlayer = deadPlayer;
            PlayerRole = playerRole;
        }

        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerScpDeathAnnouncement)handler).OnScpDeathAnnouncement(this);
        }
    }

    public class CassieCustomAnnouncementEvent : Event
    {
        public string Words { get; set; }
        public bool MonoSpaced { get; set; }
        public bool MakeNoise { get; set; }
        public bool Allow { get; set; }

        public CassieCustomAnnouncementEvent(string words, bool monospaced, bool makenoise, bool allow = true)
        {
            Words = words;
            MonoSpaced = monospaced;
            MakeNoise = makenoise;
            Allow = allow;
        }

        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerCassieCustomAnnouncement)handler).OnCassieCustomAnnouncement(this);
        }
    }

    public class CassieTeamAnnouncementEvent : Event
    {
        public char NatoLetter { get; set; }
        public int NatoNumber { get; set; }
        public int SCPsLeft { get; set; }
        public bool Allow { get; set; }

        public CassieTeamAnnouncementEvent(char natoLetter, int natoNumber, int scpsLeft, bool allow = true)
        {
            NatoLetter = natoLetter;
            NatoNumber = natoNumber;
            SCPsLeft = scpsLeft;
            Allow = allow;
        }

        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerCassieTeamAnnouncement)handler).OnCassieTeamAnnouncement(this);
        }
    }
}
