namespace SMod3.API
{
    public enum LeadingTeam : byte
    {
        FACILITY_FORCES = 0,
        CHAOS_INSURGENCY = 1,
        ANOMALIES = 2,
        DRAW = 3
    }

    public enum RoundEndStatus : byte
    {
        ON_GOING = 0,
        MTF_VICTORY = 1,
        SCP_VICTORY = 2,
        SCP_CI_VICTORY = 3,
        CI_VICTORY = 4,
        NO_VICTORY = 5,
        FORCE_END = 6,
        OTHER_VICTORY = 7
    }
}
