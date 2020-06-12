using UnityEngine;

namespace SMod3.API
{
    public abstract class Item
    {
        public abstract bool InWorld { get; }
        public abstract ItemType ItemType { get; }
        public abstract ItemCategory ItemCategory { get; }
        public abstract void Remove();
        public abstract void Drop();
        public abstract bool GetKinematic();
        public abstract object GetComponent();
        public abstract bool IsWeapon { get; }
        public abstract Weapon ToWeapon();
        public abstract bool Kinematic { get; set; }
        public abstract bool Floating { get; set; }
        public abstract Vector3 Position { get; set; }
        public abstract bool IsExists { get; }
        public abstract string[] Permissions { get; }
    }

    public abstract class DroppedItem
    {
        public abstract ItemType ItemType { get; set; }
        public abstract float Durability { get; set; }
        public abstract int Uniq { get; }
        public abstract Vector3 Position { get; set; }
        public abstract WeaponSight Sight { get; set; }
        public abstract WeaponBarrel Barrel { get; set; }
        public abstract WeaponOther Other { get; set; }
    }

    public abstract class DroppedAmmo
    {
        public abstract AmmoType AmmoType { get; set; }
        public abstract int Amount { get; set; }
        public abstract Vector3 Position { get; set; }
    }
}
