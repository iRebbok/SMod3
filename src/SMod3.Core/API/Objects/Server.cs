using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SMod3.API
{
    /// <summary>
    ///     Starting point where the command will be sent.
    /// </summary>
    public enum CommandEntry
    {
        REMOTE_ADMIN,
        SERVER_CONSOLE,
        GAME_CONSOLE
    }

    public abstract class Server : ICommandSender
    {
        public abstract Map Map { get; }
        public abstract Round Round { get; }

        /// <summary>
        ///     Server name.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Attempt to assign a null or empty value.
        /// </exception>
        public abstract string Name { get; set; }

        /// <summary>
        ///     Player list title (which is on N).
        /// </summary>
        /// <exception cref="ArgumentException"><inheritdoc cref="Name"/></exception>
        public abstract string PlayerListTitle { get; set; }

        /// <summary>
        ///     Server port.
        /// </summary>
        public abstract int Port { get; }

        /// <summary>
        ///     Server ip address.
        /// </summary>
        public abstract string IpAddress { get; }

        /// <summary>
        ///     The current number of players on the server.
        /// </summary>
        public abstract int NumPlayers { get; }

        /// <summary>
        ///     Maximum number of players on the server.
        /// </summary>
        public abstract int MaxPlayers { get; set; }

        /// <summary>
        ///     Gets readonly list of players.
        /// </summary>
        /// <remarks>
        ///     This list doesn't create a new object,
        ///     it returns all players who have a ReferenceHub (excluding a dedicated server player),
        ///     this list also contains an authorized player after calling PlayerJoinEvent.
        /// </remarks>
        public abstract IReadOnlyList<Player> Players { get; }

        /// <summary>
        ///     Gets readonly dictionary of players and their objects.
        /// </summary>
        /// <remarks><inheritdoc cref="Players"/></remarks>
        public abstract IReadOnlyDictionary<GameObject, Player> GameObjectsAndPlayers { get; }

        /// <summary>
        ///     Gets readonly dictionary of players and their ids.
        /// </summary>
        /// <remarks><inheritdoc cref="Players"/></remarks>
        public abstract IReadOnlyDictionary<int, Player> PlayerIdsAndPlayers { get; }

        /// <summary>
        ///     Gets players with specific role/s.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     None of the arguments are specified.
        /// </exception>
        /// <remarks>
        ///     This method returns a new <see cref="List{T}"/> of type <see cref="Player"/>,
        ///     you don't need to use <see cref="Enumerable.ToList{TSource}(IEnumerable{TSource})"/> or anything else,
        ///     you can just use <code>var result = (GetPlayers(RoleType.ClassD) as List&#60;Player&#62;)</code> instead.
        /// </remarks>
        public abstract IList<Player> GetPlayers(params RoleType[] role);

        /// <summary>
        ///     Gets players with specific team/s.
        /// </summary>
        /// <exception cref="ArgumentException"><inheritdoc cref="GetPlayers(RoleType[])"/></exception>
        /// <remarks><inheritdoc cref="GetPlayers(RoleType[])"/></remarks>
        public abstract IList<Player> GetPlayers(params TeamType[] team);

        /// <summary>
        ///     Gets players that match the condition.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="predicate"/> is null.
        /// </exception>
        /// <remarks><inheritdoc cref="GetPlayers(RoleType[])"/></remarks>
        public abstract IList<Player> GetPlayers(Predicate<Player> predicate);

        /// <summary>
        ///     Gets the player by condition.
        /// </summary>
        public abstract Player? GetPlayer(Predicate<Player> predicate);

        /// <summary>
        ///     Gets the player by id.
        /// </summary>
        public abstract Player? GetPlayer(int playerId);

        // If you are wondering where is 'GetAppFolder' here,
        // then I decided that this is unnecessary because
        // we now have 'PluginManager.GamePath' & 'PluginManager.BinPath'
        // so use that instead

        /// <summary>
        ///     Sends a query to one of the command entry points.
        /// </summary>
        /// <param name="sender">
        ///     Command sender, if null,
        ///     then ServerCommandSender is used.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Query is null or empty.
        /// </exception>
        public abstract void TypeCommand(string query, CommandEntry entry = CommandEntry.SERVER_CONSOLE, ICommandSender? sender = null);
    }
}
