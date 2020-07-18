using System;

using SMod3.API;
using SMod3.Core;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

namespace SMod3.Module.EventSystem.Background
{
    public static class EventHelper
    {
        /// <summary>
        ///     Converts an event to a specific type and returns it,
        ///     otherwise it throws an exception.
        /// </summary>
        /// <exception cref="InvalidOperationException"><inheritdoc cref="EventArg.CopyTo(EventArg)"/></exception>
        public static T ConvertTo<T>(EventArg other) where T : EventArg
        {
            if (other is T result)
                return result;

            throw new InvalidOperationException("The event doesn't match the specified type");
        }
    }

    #region Abstractions

    public abstract class AdminEventArg : EventArg
    {
        public ICommandSender CommandSender { get; internal set; }
    }

    public abstract class BanEventArg : EventArg
    {
        public string Issuer { get; internal set; }

        /// <summary>
        ///     Trying to get a ban issuer.
        /// </summary>
        public Player? GetIssuer() =>
            PluginManager.Manager.Server.GetPlayer(p => p.Nickname.Equals(Issuer, StringComparison.Ordinal));
    }

    public abstract class PlayerEvent : EventArg
    {
        public Player Player { get; internal set; }
    }

    public abstract class PlayerItemEvent : PlayerEvent, IAllowable
    {
        public ItemInfo Item { get; }
        public ItemType ChangeTo { get; set; }
        public bool Allow { get; set; }
    }

    public abstract class WarheadEvent : EventArg, IAllowable
    {
        /// <summary>
        ///     Is the activator, the one who caused the event.
        ///     null if activated not from the player, i.e. via RA.
        /// </summary>
        public Player? Player { get; internal set; }
        public bool Allow { get; set; }
    }

    public abstract class EmptyEventArg : EventArg
    {
        internal override void Reset() { }

        internal override void CopyTo(EventArg other) { }
    }

    #endregion

    #region Interfaces

    public interface IAllowable
    {
        public bool Allow { get; set; }
    }

    #endregion
}
