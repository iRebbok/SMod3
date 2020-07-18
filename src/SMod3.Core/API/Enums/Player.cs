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
        ///     Occurs only when <see cref="TeamRole"/> is literally empty.
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
    public enum RemoteAdminPermissions : ulong
    {
        KickingAndShortTermBanning = 1uL,
        BanningUpToDay = 2uL,
        LongTermBanning = 4uL,
        ForceclassSelf = 8uL,
        ForceclassToSpectator = 0x10,
        ForceclassWithoutRestrictions = 0x20,
        GivingItems = 0x40,
        WarheadEvents = 0x80,
        RespawnEvents = 0x100,
        RoundEvents = 0x200,
        SetGroup = 0x400,
        GameplayData = 0x800,
        Overwatch = 0x1000,
        FacilityManagement = 0x2000,
        PlayersManagement = 0x4000,
        PermissionsManagement = 0x8000,
        ServerConsoleCommands = 0x10000,
        ViewHiddenBadges = 0x20000,
        ServerConfigs = 0x40000,
        Broadcasting = 0x80000,
        PlayerSensitiveDataAccess = 0x100000,
        Noclip = 0x200000,
        AFKImmunity = 0x400000,
        AdminChat = 0x800000,
        ViewHiddenGlobalBadges = 0x1000000,
        Announcer = 0x2000000,
        Effects = 0x4000000,
        FriendlyFireDetectorImmunity = 0x8000000,
        FriendlyFireDetectorTempDisable = 0x10000000
    }

    public enum SpawnableTeamType : byte
    {
        None,
        ChaosInsurgency,
        NineTailedFox
    }
}
