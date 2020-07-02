namespace SMod3.API
{
    public enum ElevatorType : byte
    {
        LiftA = 0,
        LiftB = 1,
        GateA = 2,
        GateB = 3,
        WarheadRoom = 4,
        SCP049Chamber = 5
    }

    public enum ElevatorStatus : byte
    {
        Up,
        Down,
        Moving
    }
}
