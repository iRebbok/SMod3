using System;

namespace SMod3.API
{
    public enum DamageType : byte
    {
        NONE = 0,
        LURE = 1,
        NUKE = 2,
        WALL = 3,
        /// <summary>
        ///     Decontamination.
        /// </summary>
        DECONT = 4,
        TESLA = 5,
        FALLDOWN = 6,
        FLYING = 7,
        CONTAIN = 8,
        POCKET = 9,
        RAGDOLLLESS = 10,
        COM15 = 11,
        P90 = 12,
        E11_STANDARD_RIFLE = 13,
        MP7 = 14,
        /// <summary>
        ///     Chaos Gun.
        /// </summary>
        LOGICER = 15,
        USP = 16,
        MICROHID = 17,
        /// <summary>
        ///     Frag grenade.
        /// </summary>
        FRAG = 18,
        SCP_049 = 19,
        SCP_049_2 = 20,
        SCP_096 = 21,
        SCP_106 = 22,
        SCP_173 = 23,
        SCP_939 = 24,
        SCP_207 = 25,
        RECONTAIMENT = 26
    }

    public enum UserRank : sbyte
    {
        NONE = -1,
        PATREON_SUPPORTER = 0,
        GAME_STAFF = 1,
        PROJECT_MANAGER = 2,
        /// <summary>
        ///     Since this is the same <see cref="BAN_TEAM"/>, just invisible.
        /// </summary>
        NINJA_BAN_TEAM = 3,
        BAN_TEAM = 4,
    }

    public enum RoleType : sbyte
    {
        NONE = -1,

        SCP_173 = 0,
        CLASSD = 1,
        SPECTATOR = 2,
        SCP_106 = 3,
        NTF_SCIENTIST = 4,
        SCP_049 = 5,
        SCIENTIST = 6,
        SCP_079 = 7,
        CHAOS_INSURGENCY = 8,
        SCP_096 = 9,
        SCP_049_2 = 10,
        ZOMBIE = SCP_049_2,
        NTF_LIEUTENANT = 11,
        NTF_COMMANDER = 12,
        NTF_CADET = 13,
        TUTORIAL = 14,
        FACILITY_GUARD = 15,
        SCP_939_53 = 16,
        SCP_939_89 = 17
    }

    public enum TeamType : sbyte
    {
        /// <summary>
        ///     Occurs only when <see cref="RoleData"/> is literally empty.
        /// </summary>
        NONE = -1,

        SCP = 0,
        NINETAILFOX = 1,
        CHAOS_INSURGENCY = 2,
        SCIENTIST = 3,
        CLASSD = 4,
        SPECTATOR = 5,
        TUTORIAL = 6
    }

    /// <summary>
    ///     Player permissions at remote admin.
    /// </summary>
    [Flags]
    public enum RemoteAdminPermissions : ulong
    {
        KICKING_AND_SHORT_TERM_BANNING = 1 << 0,
        BANNING_UP_TO_DAY = 1 << 1,
        LONG_TERM_BANNING = 1 << 2,
        FORCECLASS_SELF = 1 << 3,
        FOCRCLASS_TO_SPECTATOR = 1 << 4,
        FORCECLASS_WITHOUT_RESTRICTIONS = 1 << 5,
        GIVING_ITEMS = 1 << 6,
        WARHEAD_EVENTS = 1 << 7,
        RESPAWN_EVENTS = 1 << 8,
        ROUND_EVENTS = 1 << 9,
        SET_GROUP = 1 << 10,
        GAMEPLAY_DATA = 1 << 11,
        OVERWARCH = 1 << 12,
        FACILITY_MANAGEMENT = 1 << 13,
        PLAYER_MANAGEMENT = 1 << 14,
        PERMISSION_MANAGEMENT = 1 << 15,
        SERVER_CONSOLE_COMMANDS = 1 << 16,
        VIEW_HIDDEN_BADGES = 1 << 17,
        SERVER_CONFIGS = 1 << 18,
        BROADCASTING = 1 << 19,
        PLAYER_SENSITIVE_DATA_ACCESS = 1 << 20,
        NOCLIP = 1 << 21,
        AFK_IMMUNITY = 1 << 22,
        ADMIN_CHAT = 1 << 23,
        VIEW_HIDDEN_GLOBAL_BADGES = 1 << 24,
        ANNOUNCER = 1 << 25,
        EFFECTS = 1 << 26,
        FRIENDLY_FIRE_DETECTOR_IMMUNITY = 1 << 27,
        FRIENDLY_FIRE_DETECTOR_TEMP_DISABLE = 1 << 28
    }

    public enum SpawnableTeamType : byte
    {
        NONE = 0,
        CHAOS_INSURGENCY = 1,
        NINE_TAILED_FOX = 2
    }
}
