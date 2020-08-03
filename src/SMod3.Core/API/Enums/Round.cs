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
        ON_GOING,
        MTF_VICTORY,
        SCP_VICTORY,
        SCP_CI_VICTORY,
        CI_VICTORY,
        NO_VICTORY,
        FORCE_END,
        OTHER_VICTORY
    }
}
