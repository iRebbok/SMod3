using System.Collections.Generic;

namespace SMod3.Core.API
{
    public abstract class ServerConfig
    {
        // Environment
        public abstract bool Debug { get; set; }

        public abstract int? RagdollCleanup { get; set; } // null or -1 is disabled
        public abstract int? ItemCleanup { get; set; } // null or -1 is disabled
        public abstract IList<ItemType> CleanupIgnoredItems { get; }
        public abstract bool DisableEmptyAmmoBoxes { get; set; }
        public abstract bool EnableRaServerCommands { get; set; }
        public abstract bool BypassServerCommandWhitelist { get; set; }
        public abstract IList<string> ServerCommandWhitelist { get; }

        // Game
        public abstract uint Spc268UsingTime { get; set; }
        public abstract bool ShowMaxItemsWarnings { get; set; }
        public abstract bool FloatingItemsEveryone { get; set; }
        public abstract IList<string> FloatingItemsUserIDs { get; }
        public abstract bool FriendlyFire { get; set; }
        public abstract bool BanComputerFirstPick { get; set; }
        public abstract uint StartRoundTimer { get; set; }
        public abstract uint WaitForPlayers { get; set; }
        public abstract uint AmbientsoundTimeMinPlayback { get; set; }
        public abstract uint AmbientsoundTimeMaxPlayback { get; set; }
        public abstract IList<TeamType> TelsaTriggerableTeam { get; }
        public abstract IList<RoleType> TeslaTriggerableRole { get; }
        public abstract bool UnlimitedRadioBattery { get; set; }
        public abstract bool ReduceAmmo { get; set; }
        public abstract uint TimeToRoundRestart { get; set; }

        // Player console
        public abstract ColorType ConsoleCommandNotFoundColor { get; set; }
        public abstract string ConsoleCommandNotFoundText { get; set; }

        // Damage multipliers
        public abstract float SCP106DamageMultiplier { get; set; }
        public abstract float HeadshotDamageMultiplier { get; set; }
        public abstract float LegDamageMultiplier { get; set; }

        // Broadcast
        public abstract bool BroadcastLogging { get; set; }
        public abstract bool BroadcastOutput { get; set; }
        public abstract bool BroadcastLoggingKicks { get; set; }
        public abstract bool BroadcastLoggingBans { get; set; }
        public abstract uint BroadcastKickDuration { get; set; }
        public abstract uint BroadcastBanDuration { get; set; }
        public abstract string BroadcastKickText { get; set; }
        public abstract string BroadcastBanText { get; set; }
    }
}
