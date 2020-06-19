using System.Collections.Generic;

namespace SMod3.API
{
    /// <remarks>
    ///     This item is not that
    ///     be in the player’s inventory or on the surface,
    ///     see <see cref="ItemInfo"/>.
    /// </remarks>
    public abstract class Item
    {
        /// <summary>
        ///     Default durability on all non-weapon items.
        /// </summary>
        public static readonly float DefaultDurability = -4.65664672E+11f;

        /// <inheritdoc cref="ItemType"/>
        public abstract ItemType Type { get; }
        /// <inheritdoc cref="ItemCategory"/>
        public abstract ItemCategory Category { get; }
        /// <summary>
        ///     Determines if an item is a weapon and can be converted to <see cref="Weapon"/>.
        /// </summary>
        public abstract bool IsWeapon { get; }
        /// <summary>
        ///     Item permissions.
        /// </summary>
        public abstract IReadOnlyCollection<string> Permissions { get; }
    }

    // todo: rethink weapon related shit
    public abstract class Weapon : Item
    {
        public abstract WeaponType WeaponType { get; }
        public abstract WeaponSight Sight { get; }
        public abstract WeaponBarrel Barrel { get; }
        public abstract WeaponOther Other { get; }
        public abstract float AmmoInClip { get; }
        public abstract int MaxClipSize { get; }
        public abstract AmmoType AmmoType { get; }
        public abstract DamageType DamageType { get; }
    }
}
