using System;

namespace SMod3.API
{
    /// <summary>
    ///     Door Access Requirements.
    ///     <para>
    ///         Use <see cref="Enum.HasFlag(Enum)"/> to check for permission at the door.
    ///     </para>
    /// </summary>
    [Flags]
    public enum AccessRequirements
    {
        /// <summary>
        ///     Means that the door is not accessible for interaction.
        ///     Don't confuse with locked.
        /// </summary>
        Unaccessible = 1 << 0,
        Checkpoints = 1 << 1,
        Gates = 1 << 2,
        Intercom = 1 << 3,
        AlphaWarhead = 1 << 4,
        ContainmentLevelOne = 1 << 5,
        ContainmentLevelTwo = 1 << 6,
        ContainmentLevelThree = 1 << 7,
        ArmoryLevelOne = 1 << 8,
        ArmoryLevelTwo = 1 << 9,
        ArmoryLevelThree = 1 << 10
    }

    /// <summary>
    ///     Door type.
    /// </summary>
    public enum DoorType : byte
    {
        Standard,
        HeavyGate,
        Checkpoint
    }

    /// <summary>
    ///     Type of button on the door.
    /// </summary>
    public enum ButtonType : byte
    {
        LightContainment,
        HeavyContainment,
        AccessRequired,
        Checkpoint
    }

    /// <summary>
    ///     Door status.
    /// </summary>
    public enum DoorStatus : byte
    {
        Closed,
        Open,
        Moving,
        Denied,
        Locked,
        Misc
    }
}
