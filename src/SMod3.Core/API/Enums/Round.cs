namespace SMod3.API
{
    public enum LeadingTeam : byte
    {
        FacilityForces = 0,
        ChaosInsurgency = 1,
        Anomalies = 2,
        Draw = 3
    }

    public enum ROUND_END_STATUS : byte
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
