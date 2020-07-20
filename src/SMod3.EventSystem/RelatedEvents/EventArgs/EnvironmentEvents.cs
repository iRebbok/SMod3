using System.Collections.Generic;

using SMod3.API;
using SMod3.Module.EventSystem.Background;

using UnityEngine;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

namespace SMod3.Module.EventSystem.Events
{
    public sealed class SCP914ActivateEvent : EventArg
    {
        /// <summary>
        ///     One who activated 914.
        /// </summary>
        public Player User { get; internal set; }
        public KnobSetting KnobSetting { get; set; }
        public List<Player> PlayerInputs { get; }
        public List<ISurfaceItemInfo> ItemInputs { get; }
        public Vector3 IntakePos { get; set; }
        public Vector3 OutputPos { get; set; }

        public SCP914ActivateEvent()
        {
            PlayerInputs = new List<Player>();
            ItemInputs = new List<ISurfaceItemInfo>();
        }

        internal override void Reset()
        {
            User = null;
            KnobSetting = default;
            PlayerInputs.Clear();
            ItemInputs.Clear();
            IntakePos = default;
            OutputPos = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<SCP914ActivateEvent>(other);
            target.User = User;
            target.KnobSetting = KnobSetting;
            target.PlayerInputs.AddRange(PlayerInputs);
            target.ItemInputs.AddRange(ItemInputs);
            target.IntakePos = IntakePos;
            target.OutputPos = OutputPos;
        }
    }

    #region Warhead events

    public sealed class WarheadStartCountdownEvent : WarheadEvent
    {
        /// <summary>
        ///     false if initial countdown.
        /// </summary>
        public bool IsResumed { get; internal set; }
        public bool OpenDoorsAfter { get; set; }

        internal override void Reset()
        {
            Player = null;
            Allow = true;
            IsResumed = default;
            OpenDoorsAfter = default;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<WarheadStartCountdownEvent>(other);
            target.Player = Player;
            target.Allow = Allow;
            target.IsResumed = IsResumed;
            target.OpenDoorsAfter = OpenDoorsAfter;
        }
    }

    public sealed class WarheadStopCountdownEvent : WarheadEvent
    {
        internal override void Reset()
        {
            Player = null;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<WarheadStopCountdownEvent>(other);
            target.Player = Player;
            target.Allow = Allow;
        }
    }

    public sealed class WarheadChangeLeverEvent : WarheadEvent
    {
        internal override void Reset()
        {
            Player = null;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<WarheadStopCountdownEvent>(other);
            target.Player = Player;
            target.Allow = Allow;
        }
    }

    public sealed class WarheadKeycardAccessEvent : WarheadEvent
    {
        internal override void Reset()
        {
            Player = null;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<WarheadStopCountdownEvent>(other);
            target.Player = Player;
            target.Allow = Allow;
        }
    }

    #endregion

    public sealed class GeneratorFinishEvent : EventArg
    {
        public Generator Generator { get; internal set; }

        internal override void Reset()
        {
            Generator = null;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<GeneratorFinishEvent>(other);
            target.Generator = Generator;
        }
    }

    public sealed class ScpDeathAnnouncementEvent : PlayerEvent, IAllowable
    {
        public RoleType ScpRole { get; internal set; }
        public bool Allow { get; set; }

        internal override void Reset()
        {
            Player = null;
            ScpRole = default;
            Allow = true;
        }

        internal override void CopyTo(EventArg other)
        {
            var target = EventHelper.ConvertTo<ScpDeathAnnouncementEvent>(other);
            target.Player = Player;
            target.ScpRole = ScpRole;
            target.Allow = Allow;
        }
    }
}
