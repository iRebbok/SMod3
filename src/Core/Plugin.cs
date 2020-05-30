using System;

using SMod3.Core.API;
using SMod3.Core.Logging;
using SMod3.Core.Meta;
using SMod3.Module.Commands.Console;
using SMod3.Module.Commands.RemoteAdmin;
using SMod3.Module.Config;
using SMod3.Module.EventSystem;
using SMod3.Module.Lang;
using SMod3.Module.Piping;
using SMod3.Module.Piping.Meta;

namespace SMod3.Core
{
	public abstract class Plugin : BaseLogging
	{
		#region Properties

		public PluginDefineAttribute Definer
		{
			get;
			internal set;
		}

		public PluginPipes Pipes
		{
			get;
			internal set;
		}

		public PluginStatus Status
		{
			get;
			internal set;
		}

		#endregion

		#region References

		public EventManager EventManager => EventManager.Manager;
		public PluginManager PluginManager => PluginManager.Manager;
		public LangManager LangManager => LangManager.Manager;
		public ConfigManager ConfigManager => ConfigManager.Manager;
		public RemoteAdminCommandManager RemoteAdminCommandManager => PluginManager.Manager.RemoteAdminCommandManager;
		public ConsoleCommandManager ConsoleCommandManager => PluginManager.Manager.ConsoleCommandManager;
		public Logger Logger => PluginManager.Manager.Logger;
		public Server Server => PluginManager.Manager.Server;
		public Round Round => PluginManager.Manager.Server.Round;

		#endregion

		#region Plugin Events

		/// <summary>
		///		Called first when loading the plugin.
		///		<para>
		///			It recommend registering your commands and events here.
		///		</para>
		/// </summary>
		protected virtual void Awake() { }

		/// <summary>
		///		Called when starting the plugin.
		/// </summary>
		protected virtual void OnEnable() { }

		/// <summary>
		///		Called when disabling the plugin.
		///		<para>
		///			We strongly recommend unregister all external things, such as Harmony and so on.
		///		</para>
		/// </summary>
		protected virtual void OnDisable() { }

		/// <summary>
		///		Called when destroying the plugin.
		///		<para>
		///			Before this method is called <see cref="OnDisable"/> to delete all registered items, be careful.
		///		</para>
		/// </summary>
		protected virtual void OnDestroy() { }

		#endregion

		#region Reflection Event Region

		public void InvokeEvent(string eventName) => InvokeEvent(eventName, null);
		public void InvokeEvent(string eventName, params object[] args) => InvokeExternalEvent($"{Definer.Id}.{eventName}", args);

		public void InvokeExternalEvent(string fullName) => InvokeExternalEvent(fullName, null);
		public void InvokeExternalEvent(string fullName, params object[] args)
		{
			if (string.IsNullOrEmpty(fullName))
			{
				throw new ArgumentNullException(nameof(fullName));
			}

			PipeManager.Manager.InvokeEvent(fullName, Definer.Id, args);
		}

		#endregion

		#region Base

		public override string LoggingTag => Definer.Id;

		internal void CallAwake()
		{
			PluginManager.Debug($"Invoking {nameof(Awake)}: {this}");
			Awake();
		}

		internal void CallOnEnable()
		{
			PluginManager.Debug($"Invoking {nameof(OnEnable)}: {this}");
			OnEnable();
		}

		internal void CallOnDisable()
		{
			PluginManager.Debug($"Invoking {nameof(OnDisable)}: {this}");
			OnDisable();
		}

		internal void CallOnDestroy()
		{
			PluginManager.Debug($"Invoking {nameof(OnDestroy)}: {this}");
			OnDestroy();
		}

		~Plugin()
		{
			PluginManager.Dispose(this);
		}

		public static bool operator ==(Plugin p1, Plugin p2)
		{
			if (p1 == null || p2 == null) return false;

			return p1.Definer.Id == p2.Definer.Id;
		}

		public static bool operator !=(Plugin p1, Plugin p2)
		{
			return !(p1 == p2);
		}

		public override bool Equals(object obj)
		{
			if (obj is Plugin p) return this == p;

			return Equals(obj);
		}

		public override int GetHashCode()
		{
			return Definer.Id.GetHashCode();
		}

		public override string ToString()
		{
			if (Definer == null)
			{
				return base.ToString();
			}

			return $"{Definer.Name ?? "Anonymous"} ({Definer.Id}) [{Definer.Version ?? "0.0.0"}]";
		}

		#endregion
	}
}
