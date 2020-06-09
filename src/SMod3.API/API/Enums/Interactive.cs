namespace SMod3.API
{
    public enum PocketDimensionExitType
    {
        Killer = 0,
        Exit = 1
    }

    public enum IntercomStatus
    {
        Muted,
        Restarting,
        Transmitting_Admin,
        Transmitting_Bypass,
        Transmitting,
        Ready
    }

    public enum RadioStatus
    {
        CLOSE = 0,
        SHORT_RANGE = 1,
        MEDIUM_RANGE = 2,
        LONG_RANGE = 3,
        ULTRA_RANGE = 4
    }

    public enum KnobSetting
    {
        ROUGH = 0,
        COARSE = 1,
        ONE_TO_ONE = 2,
        FINE = 3,
        VERY_FINE = 4
    }

    public enum ExperienceType
    {
        KILL_ASSIST_CLASSD = 0,
        KILL_ASSIST_CHAOS_INSURGENCY = 1,
        KILL_ASSIST_NINETAILFOX = 2,
        KILL_ASSIST_SCIENTIST = 3,
        KILL_ASSIST_SCP = 4,
        KILL_ASSIST_OTHER = 5,
        USE_DOOR = 6,
        USE_LOCKDOWN = 7,
        USE_TESLAGATE = 8,
        USE_ELEVATOR = 9,
        CHEAT = 10
    }

    public enum Scp079InteractionType
    {
        CAMERA = 0,
        SPEAKER = 4
    }

    public enum RoomType
    {
        UNDEFINED = 0,
        WC00 = 1,
        SCP_914 = 2,
        AIRLOCK_00 = 3,
        AIRLOCK_01 = 4,
        CHECKPOINT_A = 5,
        CHECKPOINT_B = 6,
        HCZ_ARMORY = 7,
        SERVER_ROOM = 8,
        MICROHID = 9,
        NUKE = 10,
        SCP_012 = 11,
        SCP_049 = 12,
        SCP_079 = 13,
        SCP_096 = 14,
        SCP_106 = 15,
        SCP_173 = 16,
        SCP_372 = 17,
        SCP_939 = 18,
        ENTRANCE_CHECKPOINT = 19,
        TESLA_GATE = 20,
        PC_SMALL = 21,
        PC_LARGE = 22,
        GATE_A = 23,
        GATE_B = 24,
        CAFE = 25,
        INTERCOM = 26,
        DR_L = 27,
        STRAIGHT = 28,
        CURVE = 29,
        T_INTERSECTION = 30,
        X_INTERSECTION = 31,
        LCZ_ARMORY = 32,
        CLASS_D_CELLS = 33,
        CUBICLES = 34
    }

    public enum ZoneType
    {
        UNDEFINED = 0,
        LCZ = 1,
        HCZ = 2,
        ENTRANCE = 3
    }
}
