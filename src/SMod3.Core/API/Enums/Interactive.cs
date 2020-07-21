namespace SMod3.API
{
    public enum PocketDimensionExitType : byte
    {
        Killer = 0,
        Exit = 1
    }

    public enum IntercomStatus : byte
    {
        Muted,
        Restarting,
        Transmitting_Admin,
        Transmitting_Bypass,
        Transmitting,
        Ready
    }

    public enum RadioStatus : byte
    {
        CLOSE = 0,
        SHORT_RANGE = 1,
        MEDIUM_RANGE = 2,
        LONG_RANGE = 3,
        ULTRA_RANGE = 4
    }

    public enum KnobSetting : byte
    {
        ROUGH = 0,
        COARSE = 1,
        ONE_TO_ONE = 2,
        FINE = 3,
        VERY_FINE = 4
    }

    public enum ExperienceType : byte
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

    public enum Scp079InteractionType : byte
    {
        CAMERA = 0,
        SPEAKER = 4
    }

    public enum ZoneType : byte
    {
        /// <summary>
        ///     Could not determine room zone.
        /// </summary>
        UNDEFINED = 0,
        LCZ = 1,
        HCZ = 2,
        ENTRANCE = 3
    }

    public enum BanType : sbyte
    {
        NONE = -1,
        UserId,
        IP
    }
}