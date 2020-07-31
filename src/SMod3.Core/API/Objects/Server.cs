using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public abstract ushort Port { get; }

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
        public abstract ReadOnlyCollection<Player> Players { get; }

        /// <summary>
        ///     Gets readonly dictionary of players and their objects.
        /// </summary>
        /// <remarks><inheritdoc cref="Players"/></remarks>
        public abstract ReadOnlyDictionary<GameObject, Player> GameObjectsAndPlayers { get; }

        /// <summary>
        ///     Gets readonly dictionary of players and their ids.
        /// </summary>
        /// <remarks><inheritdoc cref="Players"/></remarks>
        public abstract ReadOnlyDictionary<int, Player> PlayerIdsAndPlayers { get; }

        /// <summary>
        ///     Gets players with specific role/s.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     None of the arguments are specified.
        /// </exception>
        public IEnumerable<Player> GetPlayers(params RoleType[] role)
        {
            if (role is null || role.Length == 0)
                throw new ArgumentException(nameof(role));

            return Players.Where(p => Array.Exists(role, r => r == p.TeamRole.Role));
        }

        /// <summary>
        ///     Gets players with specific team/s.
        /// </summary>
        /// <exception cref="ArgumentException"><inheritdoc cref="GetPlayers(RoleType[])"/></exception>
        public IEnumerable<Player> GetPlayers(params TeamType[] team)
        {
            if (team is null || team.Length == 0)
                throw new ArgumentException(nameof(team));

            return Players.Where(p => Array.Exists(team, t => t == p.TeamRole.Team));
        }

        /// <summary>
        ///     Gets players that match the condition.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="predicate"/> is null.
        /// </exception>
        public IEnumerable<Player> GetPlayers(Predicate<Player> predicate)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return Players.Where(p => predicate(p));
        }

        /// <summary>
        ///     Gets the player by condition.
        /// </summary>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="GetPlayers(Predicate{Player})" /></exception>
        public Player? GetPlayer(Predicate<Player> predicate)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            return Players.FirstOrDefault(p => predicate(p));
        }

        /// <summary>
        ///     Gets the player by id.
        /// </summary>
        public Player? GetPlayer(int playerId)
        {
            PlayerIdsAndPlayers.TryGetValue(playerId, out var result);
            return result;
        }

        /// <summary>
        ///     Gets the player by <see cref="GameObject"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="Dictionary{TKey, TValue}.TryGetValue(TKey, out TValue)" /></exception>
        public Player? GetPlayer(GameObject playerObject)
        {
            GameObjectsAndPlayers.TryGetValue(playerObject, out var result);
            return result;
        }

        /// <summary>
        ///     Tries to get the plyaer by condition.
        /// </summary>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="GetPlayers(Predicate{Player})" /></exception>
        public bool TryGetPlayer(Predicate<Player> predicate, out Player? player)
        {
            player = GetPlayer(predicate);
            return player is null;
        }

        /// <summary>
        ///     Tries to get the player by id.
        /// </summary>
        public bool TryGetPlayer(int playerId, out Player? player)
        {
            return PlayerIdsAndPlayers.TryGetValue(playerId, out player);
        }

        /// <summary>
        ///     Tries to get the plyaer by <see cref="GameObject"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><inheritdoc cref="Dictionary{TKey, TValue}.TryGetValue(TKey, out TValue)" /></exception>
        public bool TryGetPlayer(GameObject playerObject, out Player? player)
        {
            return GameObjectsAndPlayers.TryGetValue(playerObject, out player);
        }

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
