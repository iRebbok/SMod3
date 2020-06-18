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
        /// <remarks>
        ///     When assigning <see cref="ItemType.NONE"/>, the item will be destroyed.
        /// </remarks>
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
        ///     Indicates when the item is on a surface
        ///     and can be converted to <see cref="ISurfaceItemInfo"/>,
        ///     otherwise to <see cref="InventoryItemInfo"/>.
        /// </summary>
        bool InWorld { get; }

        /// <summary>
        ///     Indicates that the item still exists and can be interacted with.
        /// </summary>
        bool StillExists { get; }

        /// <summary>
        ///     Deletes an item.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     Item destroyed.
        ///     
        ///     When an item has been picked up by a player
        ///     or has been dropped by a player,
        ///     this way you can no longer interact with it.
        /// </exception>
        void Delete();

        /// <summary>
        ///     Gets an item of this type.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     
        /// </exception>
        Item GetItem();
    }

    /// <summary>
    ///     Information about the item that is on the surface.
    /// </summary>
    public interface ISurfaceItemInfo : ItemInfo
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
    }

    // todo: implement weapon info
}
