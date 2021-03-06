using System;

using UnityEngine;

namespace SMod3.API
{
    /// <summary>
    ///     Information about an item that is in the player’s inventory or on the surface.
    /// </summary>
    public interface ItemInfo
    {
        /// <summary>
        ///     Item type.
        /// </summary>
        /// <exception cref="ArgumentException"><inheritdoc cref="Inventory.GiveItem(ItemType)" /></exception>
        /// <exception cref="InvalidOperationException">
        ///     Attempted interaction after deletion.
        /// </exception>
        ItemType Type { get; set; }

        /// <summary>
        ///     Gets or sets durability to the item.
        /// </summary>
        /// <remarks>
        ///     Durability, although it is a floating point value,
        ///     it rounds the final set value and applies only to weapons.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        ///     <inheritdoc cref="ItemInfo.Type"/>
        ///     Attempting to assign a value to a non-weapon item.
        /// </exception>
        float Durability { get; set; }

        /// <summary>
        ///     Item location.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Item has been destroyed.
        /// </exception>
        ItemLocation Location { get; }

        /// <summary>
        ///     Indicates that the item still exists and can be interacted with.
        /// </summary>
        bool StillExists { get; }

        /// <summary>
        ///     Deletes an item.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Item has been destroyed.
        ///     When an item has been picked up by a player
        ///     or has been dropped by a player,
        ///     this way you can no longer interact with it.
        /// </exception>
        void Delete();

        /// <summary>
        ///     Gets an item of this type.
        /// </summary>
        Item GetItem();
    }

    /// <summary>
    ///     Information about the item that is on the surface.
    /// </summary>
    public interface ISurfaceItemInfo : ItemInfo, IGenericApiObject
    {
        /// <summary>
        ///     Gets or sets an item’s position.
        /// </summary>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="ItemInfo.Type"/></exception>
        Vector3 Position { get; set; }

        /// <summary>
        ///     Gets or sets the effects of physics to the item.
        ///     See also <a href="https://docs.unity3d.com/2019.3/Documentation/ScriptReference/Rigidbody-isKinematic.html">Unity Docs</a>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="ItemInfo.Type"/></exception>
        bool IsKinematic { get; set; }

        /// <summary>
        ///     Controls whether gravity affects this rigidbody.
        ///     See also <a href="https://docs.unity3d.com/2019.3/Documentation/ScriptReference/Rigidbody-useGravity.html">Unity Docs</a>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="ItemInfo.Type"/></exception>
        bool UseGravity { get; set; }

        /// <summary>
        ///     Gets the item's rigidbody.
        /// </summary>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="ItemInfo.Type"/></exception>
        Rigidbody GetRigidbody();
    }

    /// <summary>
    ///     Information about the item in the player’s inventory.
    /// </summary>
    public interface InventoryItemInfo : ItemInfo
    {
        /// <summary>
        ///     Unique identifier of information about the item
        ///     that is in the inventory (the player’s current item is found on it).
        /// </summary>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="ItemInfo.Type"/></exception>
        int Uniq { get; }

        /// <summary>
        ///     Drops an item from inventory.
        /// </summary>
        /// <returns>
        ///     Source of the dropped item.
        /// </returns>
        ISurfaceItemInfo Drop();
    }

    // todo: implement weapon info & locker API with the locker item info
}
