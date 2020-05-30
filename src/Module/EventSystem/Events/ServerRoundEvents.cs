using SMod3.Core.API;
using SMod3.Module.EventHandlers;
using SMod3.Module.EventSystem.EventHandlers.Meta;
using SMod3.Module.EventSystem.Events.Meta;

namespace SMod3.Module.EventSystem.Events
{
	public abstract class ServerEvent : Event
	{
		public Server Server { get; }

		public ServerEvent(Server server)
		{
			Server = server;
		}
	}

	public class RoundStartEvent : ServerEvent
	{
		public RoundStartEvent(Server server) : base(server) { }

		public override void ExecuteHandler(IEventHandler handler)
		{
			((IEventHandlerRoundStart)handler).OnRoundStart(this);
		}
	}

	public class RoundEndEvent : ServerEvent
	{
		public LeadingTeam LeadingTeam { get; set; }
		public Round Round { get; }
		public ROUND_END_STATUS Status { get; }

		public RoundEndEvent(Server server, Round round, ROUND_END_STATUS status, LeadingTeam leading) : base(server)
		{
			Round = round;
			Status = status;
			LeadingTeam = leading;
		}

		public override void ExecuteHandler(IEventHandler handler)
		{
			((IEventHandlerRoundEnd)handler).OnRoundEnd(this);
		}
	}

	public abstract class ConnectionEvent : Event
	{
		public Connection Connection { get; }

		public ConnectionEvent(Connection connection)
		{
			Connection = connection;
		}
	}

	public class ConnectEvent : ConnectionEvent
	{
		public ConnectEvent(Connection connection) : base(connection)
		{
		}

		public override void ExecuteHandler(IEventHandler handler)
		{
			((IEventHandlerConnect)handler).OnConnect(this);
		}
	}

	public class DisconnectEvent : Event
	{
		public DisconnectEvent()
		{
		}

		public override void ExecuteHandler(IEventHandler handler)
		{
			((IEventHandlerDisconnect)handler).OnDisconnect(this);
		}
	}

	public class CheckRoundEndEvent : ServerEvent
	{
		public Round Round { get; }
		public ROUND_END_STATUS Status { get; set; }

		public CheckRoundEndEvent(Server server, Round round) : base(server)
		{
			Round = round;
		}

		public override void ExecuteHandler(IEventHandler handler)
		{
			((IEventHandlerCheckRoundEnd)handler).OnCheckRoundEnd(this);
		}
	}

	public class WaitingForPlayersEvent : ServerEvent
	{
		public WaitingForPlayersEvent(Server server) : base(server)
		{
		}

		public override void ExecuteHandler(IEventHandler handler)
		{
			((IEventHandlerWaitingForPlayers)handler).OnWaitingForPlayers(this);
		}
	}

	public class RoundRestartEvent : ServerEvent
	{
		public RoundRestartEvent(Server server) : base(server)
		{
		}

		public override void ExecuteHandler(IEventHandler handler)
		{
			((IEventHandlerRoundRestart)handler).OnRoundRestart(this);
		}
	}

	public class SetServerNameEvent : ServerEvent
	{
		public string ServerName;

		public SetServerNameEvent(Server server, string ServerName) : base(server)
		{
			this.ServerName = ServerName;
		}

		public override void ExecuteHandler(IEventHandler handler)
		{
			((IEventHandlerSetServerName)handler).OnSetServerName(this);
		}
	}

	public class UpdateEvent : Event
	{
		public UpdateEvent()
		{
		}

		public override void ExecuteHandler(IEventHandler handler)
		{
			((IEventHandlerUpdate)handler).OnUpdate(this);
		}
	}

	public class FixedUpdateEvent : Event
	{
		public FixedUpdateEvent()
		{
		}

		public override void ExecuteHandler(IEventHandler handler)
		{
			((IEventHandlerFixedUpdate)handler).OnFixedUpdate(this);
		}
	}

	public class LateUpdateEvent : Event
	{
		public LateUpdateEvent()
		{
		}

		public override void ExecuteHandler(IEventHandler handler)
		{
			((IEventHandlerLateUpdate)handler).OnLateUpdate(this);
		}
	}

	public class SceneChangedEvent : Event
	{
		public string SceneName { get; }

		public SceneChangedEvent(string sceneName)
		{
			SceneName = sceneName;
		}

		public override void ExecuteHandler(IEventHandler handler)
		{
			((IEventHandlerSceneChanged)handler).OnSceneChanged(this);
		}
	}

	public class SetSeedEvent : Event
	{
		public int Seed { get; set; }

		public SetSeedEvent(int seed)
		{
			Seed = seed;
		}

		public override void ExecuteHandler(IEventHandler handler)
		{
			((IEventHandlerSetSeed)handler).OnSetSeed(this);
		}
	}
}
