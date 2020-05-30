using System;
using System.Collections.Generic;
using SMod3.Module.Commands.Meta;
using SMod3.Core.API;

namespace SMod3.Core.API
{
	[Flags]
	public enum SearchFilter
	{
		ALL = 1 << 0,
		NICKNAME = 1 << 1,
		USERID = 1 << 2,
		IP = 1 << 3,
		PLAYERID = 1 << 4
	}

	public abstract class Server : ICommandSender
	{
		public abstract string Name { get; set; }
		public abstract int Port { get; }
		public abstract string IpAddress { get; }
		public abstract Round Round { get; }
		public abstract Map Map { get; }
		public abstract ServerConfig ServerConfig { get; }
		public abstract int NumPlayers { get; }
		public abstract int MaxPlayers { get; set; }
		public abstract string PlayerListTitle { get; set; }
		public abstract Time Time { get; }

		public abstract IList<Player> GetPlayers();
		public abstract IList<Player> GetPlayers(string filter);
		public abstract IList<Player> GetPlayers(string filter, SearchFilter searchFilter);
		public abstract IList<Player> GetPlayers(params RoleType[] role);
		public abstract IList<Player> GetPlayers(params TeamType[] team);
		public abstract IList<Player> GetPlayers(Predicate<Player> predicate);
		public abstract Player GetPlayer(int playerId);
		public abstract IList<Connection> GetConnections(string filter = "");
		public abstract IList<TeamRole> GetRoles(string filter = "");
		public abstract string GetAppFoler(bool addSeparetor = true, bool serverConfig = false, string centralConfig = "");

		public abstract bool BanUserId(string username, string userId, int duration, string reason = "", string issuer = "Server");
		public abstract bool BanIpAddress(string username, string ipAddress, int duration, string reason = "", string issuer = "Server");
	}
}
