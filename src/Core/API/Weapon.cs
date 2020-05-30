namespace SMod3.Core.API
{
    public abstract class Weapon
    {
        public abstract WeaponType WeaponType { get; }
        public abstract WeaponSight Sight { get; set; }
        public abstract WeaponBarrel Barrel { get; set; }
        public abstract WeaponOther Other { get; set; }
        public abstract float AmmoInClip { get; set; }
        public abstract int MaxClipSize { get; }
        public abstract AmmoType AmmoType { get; }
        public abstract DamageType DamageType { get; }
        public abstract object GetComponent();
        public abstract Item ToItem();
    }
}
