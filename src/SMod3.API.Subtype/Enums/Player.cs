namespace SMod3.API
{

    public enum AuthType
    {
        SERVER,
        GAMESTAFF
    }

    public enum DamageType
    {
        NONE = 0,
        LURE = 1,
        NUKE = 2,
        WALL = 3,
        DECONT = 4, // Decontamination
        TESLA = 5, // Tesla
        FALLDOWN = 6,
        FLYING = 7,
        CONTAIN = 8,
        POCKET = 9,
        RAGDOLLLESS = 10,
        COM15 = 11,
        P90 = 12,
        E11_STANDARD_RIFLE = 13,
        MP7 = 14,
        LOGICER = 15, // Chaos Gun
        USP = 16,
        MICROHID = 17,
        FRAG = 18, // Frag grenade
        SCP_049 = 19,
        SCP_049_2 = 20,
        SCP_096 = 21,
        SCP_106 = 22,
        SCP_173 = 23,
        SCP_939 = 24,
        SCP_207 = 25,
        RECONTAIMENT = 26
    }

    public enum UserRank
    {
        ADMIN = 5,
        PROJECT_MANAGER = 4,
        GAME_STAFF = 3,
        BETATESTER = 2,
        PATREON_SUPPORTED = 1,
        NONE = 0
    }

    public enum RoleType
    {
        UNASSIGNED = -1,

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
        ZOMBIE = 10,
        NTF_LIEUTENANT = 11,
        NTF_COMMANDER = 12,
        NTF_CADET = 13,
        TUTORIAL = 14,
        FACILITY_GUARD = 15,
        SCP_939_53 = 16,
        SCP_939_89 = 17
    }

    public enum TeamType
    {
        NONE = -1,
        SCP = 0,
        NINETAILFOX = 1,
        CHAOS_INSURGENCY = 2,
        SCIENTIST = 3,
        CLASSD = 4,
        SPECTATOR = 5,
        TUTORIAL = 6
    }
}
