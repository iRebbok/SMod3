namespace SMod3.API
{
    public enum AmmoType : sbyte
    {
        /// <summary>
        ///     Has no base in-game.
        /// </summary>
        NONE = -1,
        /// <summary>
        ///     Epsilon-11 Standard Rifle (Type 0).
        /// </summary>
        AMMO556 = 0,
        /// <summary>
        ///     MP7, Logicer (Type 1).
        /// </summary>
        AMMO762 = 1,
        /// <summary>
        ///     COM15, P90 (Type 2).
        /// </summary>
        AMMO9MM = 2
    }

    public enum AttachmentType : sbyte
    {
        NONE = -1,
        SIGHT = 0,
        BARREL = 1,
        OTHER = 2
    }

    public enum WeaponBarrel : byte
    {
        NONE = 0,
        SUPPRESSOR = 1,
        OIL_FILTER = 2,
        MUZZLE_BREAK = 3,
        HEAVY_BARREL = 4,
        MUZZLE_BOOSTER = 5
    }

    public enum WeaponOther : byte
    {
        NONE = 0,
        FLASHLIGHT = 1,
        LASER = 2,
        AMMO_COUNTER = 3,
        GYROSCOPIC_STABILIZER = 4
    }

    public enum WeaponSight : byte
    {
        NONE = 0,
        RED_DOT = 1,
        HOLO_SIGHT = 2,
        NIGHT_VISION = 3,
        SNIPER_SCOPE = 4,
        COLLIMATOR = 5
    }

    public enum WeaponSound : byte
    {
        COM15 = 0,
        P90 = 1,
        E11_STANDARD_RIFLE = 2,
        MP7 = 3,
        LOGICER = 4,
        USP = 5,
    }

    public enum WeaponType : byte
    {
        COM15 = (int)ItemType.GUN_COM15,
        MICROHID = (int)ItemType.MICRO_HID,
        E11_STANDARD_RIFLE = (int)ItemType.GUN_E11_SR,
        P90 = (int)ItemType.GUN_PROJECT90,
        MP7 = (int)ItemType.GUN_MP7,
        LOGICER = (int)ItemType.GUN_LOGICER,
        USP = (int)ItemType.GUN_USP
    }
}
