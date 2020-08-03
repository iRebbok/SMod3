using System;

namespace SMod3.API
{
    /// <summary>
    ///     Door Access Requirements.
    /// </summary>
    [Flags]
    public enum DoorAccessRequirements
    {
        /// <summary>
        ///     Means that the door is not accessible for interaction.
        ///     Don't confuse with locked.
        /// </summary>
        UNACCESSIBLE = 1 << 0,
        CHECKPOINTS = 1 << 1,
        GATES = 1 << 2,
        INTERCOM = 1 << 3,
        ALPHA_WARHEAD = 1 << 4,
        CONTAINMENT_LEVEL_ONE = 1 << 5,
        CONTAINMENT_LEVEL_TWO = 1 << 6,
        CONTAINMENT_LEVEL_THREE = 1 << 7,
        ARMY_LEVEL_ONE = 1 << 8,
        ARMY_LEVEL_TWO = 1 << 9,
        ARMY_LEVEL_THREE = 1 << 10
    }

    /// <summary>
    ///     Door type.
    /// </summary>
    public enum DoorType : byte
    {
        STANDARD,
        HEAVY_GATE,
        CHECKPOINT
    }

    /// <summary>
    ///     Type of button on the door.
    /// </summary>
    public enum DoorButtonType : byte
    {
        LIGHT_CONTAINMENT,
        HEAVY_CONTAIMENT,
        ACCESS_REQUIRED,
        CHECKPOINT
    }

    /// <summary>
    ///     Door status.
    /// </summary>
    public enum DoorStatus : byte
    {
        CLOSED,
        OPEN,
        MOVING,
        DENIED,
        LCOKED,
        MISC
    }
}
