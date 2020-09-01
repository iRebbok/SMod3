namespace SMod3.API
{
    public enum ElevatorType : byte
    {
        LIFT_A = 0,
        LIFT_B = 1,
        GATE_A = 2,
        GATE_B = 3,
        WARHEAD_ROOM = 4,
        SCP049_CHAMBER = 5
    }

    public enum ElevatorStatus : byte
    {
        UP = 0,
        DOWN = 1,
        MOVING = 2
    }
}
