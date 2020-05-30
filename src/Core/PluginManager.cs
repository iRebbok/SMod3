using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using SMod3.Core.API;
using SMod3.Core.Logging;
using SMod3.Core.Meta;
using SMod3.Module.Commands.Console;
using SMod3.Module.Commands.RemoteAdmin;
using SMod3.Module.Config;
using SMod3.Module.EventSystem;
using SMod3.Module.Lang;
using SMod3.Module.Permissions;
using SMod3.Module.Piping;
using SMod3.Module.Piping.Meta;

namespace SMod3.Core
{
	public enum PluginStatus
	{
		ENABLED,
		DISABLED
	}

	public class PluginManager : BaseManager
	{
		/// <remarks>
		///		Never use this constant to check the version.
		/// </remarks>
		public const string _VERSION = "4.0.1.0";

		public static readonly int SMOD_MAJOR = int.Parse(_VERSION.Split('.')[0]);
		public static readonly int SMOD_MINOR = int.Parse(_VERSION.Split('.')[1]);
		public static readonly int SMOD_REVISION = int.Parse(_VERSION.Split('.')[2]);

		public static readonly string SMOD_BUILD = ((char)('A' + int.Parse(_VERSION.Split('.')?[3] ?? "0"))).ToString();

		public static readonly string DEPENDENCY_FOLDER = "dependencies";

		public const BindingFlags ALL_BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		public static string GetSmodVersion()
		{
			return $"{SMOD_MAJOR}.{SMOD_MINOR}.{SMOD_REVISION}";
		}

		private readonly List<Plugin> plugins = new List<Plugin>();
		private readonly Dictionary<string, Assembly> pluginAssemblies = new Dictionary<string, Assembly>();
		private readonly List<Assembly> dependencies = new List<Assembly>();

		public override string LoggingTag => "PLUGIN_MANAGER";

		private string executingPath;
		/// <summary>
		///		Location of the Smod2.dll.
		///		<para>
		///			It is initially located in <c>Game/Atlas/bin</c>
		///		</para>
		/// </summary>
		public string ExecutingPath
		{
			get
			{
				if (string.IsNullOrEmpty(executingPath))
				{
					executingPath = Assembly.GetExecutingAssembly().Location;
				}
				return executingPath;
			}
		}

		private string gamePath;
		/// <summary>
		///		Path with SCPSL.exe or SCPSL.x86_64 (if uses linux).
		/// </summary>
		public string GamePath
		{
			get
			{
				if (string.IsNullOrEmpty(gamePath))
				{
					gamePath = Path.Combine(ExecutingPath, "..", "..");
				}
				return gamePath;
			}
		}

		private string pluginPath;
		/// <summary>
		///		Path to the plugin folder.
		/// </summary>
		public string PluginPath
		{
			get
			{
				if (string.IsNullOrEmpty(pluginPath))
				{
					pluginPath = Path.Combine(GamePath, "sm_plugins");
				}
				return pluginPath;
			}
		}

		private string dependencyPath;
		/// <summary>
		///		Path to the plugin dependencies.
		/// </summary>
		public string DependencyPath
		{
			get
			{
				if (string.IsNullOrEmpty(dependencyPath))
				{
					dependencyPath = Path.Combine(PluginPath, DEPENDENCY_FOLDER);
				}
				return dependencyPath;
			}
		}

		public IEnumerable<Plugin> EnabledPlugins
		{
			get
			{
				return plugins.Where(plugin => plugin.Status == PluginStatus.ENABLED).OrderByDescending(plugin => plugin.Definer.LoadPriority);
			}
		}

		public IEnumerable<Plugin> DisabledPlugins
		{
			get
			{
				return plugins.Where(plugin => plugin.Status == PluginStatus.DISABLED).OrderByDescending(plugin => plugin.Definer.LoadPriority);
			}
		}

		public IEnumerable<Plugin> Plugins
		{
			get
			{
				return plugins.OrderByDescending(plugin => plugin.Definer.LoadPriority).ThenBy(plugin => plugin.Status);
			}
		}

		public PermissionManager PermissionsManager => PermissionManager.Manager;

		private Server server;
		public Server Server
		{
			get => server;
			set
			{
				if (server == null)
				{
					server = value;
				}
			}
		}
		public Round Round => Server.Round;

		public EventManager EventManager => EventManager.Manager;
		public LangManager LangManager => LangManager.Manager;
		public ConfigManager ConfigManager => ConfigManager.Manager;
		public Logger Logger => Logger.Singleton;
		public RemoteAdminCommandManager RemoteAdminCommandManager => RemoteAdminCommandManager.Manager;
		public ConsoleCommandManager ConsoleCommandManager => ConsoleCommandManager.Manager;

		public static PluginManager Manager { get; } = new PluginManager();

		public Plugin GetEnabledPlugin(string id) =>
			plugins.FirstOrDefault(plugin => plugin.Status == PluginStatus.ENABLED && plugin.Definer.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));

		public Plugin GetDisabledPlugin(string id) =>
			plugins.FirstOrDefault(plugin => plugin.Status == PluginStatus.DISABLED && plugin.Definer.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));

		public Plugin GetPlugin(string id) =>
			plugins.FirstOrDefault(plugin => plugin.Definer.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));

		public PluginStatus? GetPluginStatus(string id)
		{
			Plugin plugin;
			if (string.IsNullOrEmpty(id) || (plugin = GetPlugin(id)) == null)
			{
				return null;
			}

			return GetPluginStatus(plugin);
		}

		public PluginStatus? GetPluginStatus(Plugin plugin)
		{
			if (plugin == null)
			{
				return null;
			}

			return plugin.Status;
		}

		/// <summary>
		///		Sets the status of the plugin and returns the old status.
		/// </summary>
		public PluginStatus? ChangePluginStatus(Plugin plugin, PluginStatus status)
		{
			if (plugin == null) return null;
			PluginStatus oldStatus = plugin.Status;
			plugin.Status = status;
			return oldStatus;
		}

		public bool EnablePlugins()
		{
			return DisabledPlugins.All(plugin => EnablePlugin(plugin));
		}

		public bool EnablePlugin(Plugin plugin)
		{
			if (plugin == null || GetPluginStatus(plugin) == PluginStatus.ENABLED) return false;

			try
			{
				Info($"Enabling plugin: {plugin}");
				try
				{
					plugin.CallOnEnable();
				}
				catch (Exception e)
				{
					Error($"Enabling the plugin ({plugin}) throw an exception: {e.InnerException} {e.Message}");
					Debug(e.StackTrace);
					return false;
				}
				ChangePluginStatus(plugin, PluginStatus.ENABLED);
				Info($"Enabled plugin: {plugin}");
			}
			catch (Exception e)
			{
				Error($"Somehow of enabling the plugin ({plugin}) throw an exception: {e.InnerException} {e.Message}");
				Debug(e.StackTrace);
				return false;
			}

			return true;
		}

		public bool EnablePlugin(string id)
		{
			Plugin plugin;
			if ((plugin = GetPlugin(id)) == null)
			{
				return false;
			}

			return EnablePlugin(plugin);
		}

		public bool DisablePlugins()
		{
			return EnabledPlugins.All(plugin => DisablePlugin(plugin));
		}

		public bool DisablePlugin(Plugin plugin)
		{
			if (plugin == null || GetPluginStatus(plugin) == PluginStatus.DISABLED) return false;

			try
			{
				Info($"Disabling plugin: {plugin}");
				try
				{
					plugin.CallOnDisable();
				}
				catch (Exception e)
				{
					Error($"Disabling the plugin ({plugin}) throw an exception: {e.InnerException} {e.Message}");
					Debug(e.StackTrace);

					return false;
				}
				ChangePluginStatus(plugin, PluginStatus.DISABLED);
				Info($"Disabled plugin: {plugin}");
			}
			catch (Exception e)
			{
				Error($"Somehow of disabling the plugin ({plugin}) throw an exception: {e.InnerException} {e.Message}");
				Debug(e.StackTrace);
				return false;
			}

			return true;
		}

		public bool DisablePlugin(string id)
		{
			Plugin plugin;
			if ((plugin = GetPlugin(id)) == null)
			{
				return false;
			}

			return DisablePlugin(plugin);
		}

		public void DestroyPlugin(Plugin plugin)
		{
			if (plugin == null) return ;

			try
			{
				if (GetPluginStatus(plugin) != PluginStatus.DISABLED) DisablePlugin(plugin);

				Info($"Destroying pluging: {plugin}");
				try
				{
					plugin.CallOnDestroy();
				}
				catch (Exception e)
				{
					Error($"Destroying the plugin ({plugin}) throw an exception: {e.InnerException} {e.Message}");
					Debug(e.StackTrace);
					// We can't destroy the plugin's assembly because it will cause a lot of problems
					return;
				}

				plugins.Remove(plugin);
				string pluginId = plugin.Definer.Id;
				if (pluginAssemblies.TryGetValue(pluginId, out Assembly pluginAssembly))
				{
					pluginAssembly = null;
					pluginAssemblies.Remove(pluginId);
				}
			}
			catch (Exception e)
			{
				Error($"Somehow of destroying plugin ({plugin}) throw an exception: {e.InnerException} {e.Message}");
				Debug(e.StackTrace);
			}
		}

		public void DestroyPlugin(string id)
		{
			Plugin plugin;
			if ((plugin = GetPlugin(id)) == null)
			{
				return;
			}

			DestroyPlugin(plugin);
		}

		public override void Dispose(Plugin plugin)
		{
			if (plugin == null) return;

			Info($"Disposing plugin: {plugin}");

			Debug($"Unloading remote admin commands: {plugin}");
			RemoteAdminCommandManager.Dispose(plugin);

			Debug($"Unloading console commands: {plugin}");
			ConsoleCommandManager.Dispose(plugin);

			Debug($"Unloading event handlers: {plugin}");
			EventManager.Manager.Dispose(plugin);

			Debug($"Unloading configs: {plugin}");
			ConfigManager.Manager.Dispose(plugin);

			Debug($"Unloading translations: {plugin}");
			LangManager.Manager.Dispose(plugin);

			Debug($"Unloading pipe imports/exports: {plugin}");
			PipeManager.Manager.Dispose(plugin);

			Info($"Disposed plugin: {plugin}");
		}

		public void Dispose(string id)
		{
			Plugin plugin;
			if ((plugin = GetPlugin(id)) == null)
			{
				return;
			}

			Dispose(plugin);
		}

		public bool IsNotUniqueId(string id)
		{
			return !plugins.Any(pl => pl.Definer.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
		}

		public void Load()
		{
			LoadDependencies();
			LoadPlugins();
		}

		public void LoadDependencies()
		{
			string dependencyPath = DependencyPath;
			if (!Directory.Exists(dependencyPath))
			{
				Debug($"No dependencies for directory: {dependencyPath}");
				return;
			}

			DirectoryInfo dir = new DirectoryInfo(dependencyPath);
			foreach (FileInfo file in dir.GetFiles("*.dll"))
			{
				Info($"Loading plugin dependency: {file.FullName}");
				try
				{
					Assembly assembly = Assembly.Load(File.ReadAllBytes(file.FullName));
					LoadDependency(assembly);
				}
				catch (Exception e)
				{
					Warn($"Failed to load dependency: {file.FullName}");
					Debug(e.Message);
					Debug(e.StackTrace);
				}
			}
		}

		public void LoadPlugins()
		{
			string pluginPath = PluginPath;
			if (!Directory.Exists(pluginPath))
			{
				Debug($"No plugins for directory: {pluginPath}");
				Directory.CreateDirectory(pluginPath);
				return;
			}

			DirectoryInfo dirInfo = new DirectoryInfo(pluginPath);
			foreach (FileInfo file in dirInfo.GetFiles("*.dll"))
			{
				Info($"Loading plugin assembly: {file.FullName}");
				try
				{
					Assembly assembly = Assembly.Load(File.ReadAllBytes(file.FullName));
					LoadPlugin(assembly);
				}
				catch (Exception e)
				{
					Warn($"Failed to load assembly: {file.FullName}");
					Debug(e.Message);
					Debug(e.StackTrace);
				}
			}

		}

		public void LoadDependency(Assembly assembly)
		{
			if (assembly == null)
			{
				Warn($"Attempt to identify a null assembly");
			}

			if (dependencies.Contains(assembly))
			{
				Warn("Attempt to identify an already identified assembly");
			}

			dependencies.Add(assembly);
		}

		public void LoadPlugin(Assembly assembly)
		{
			if (assembly == null)
			{
				Warn("Attempt to identify null assembly for plugins");
			}

			if (pluginAssemblies.ContainsValue(assembly))
			{
				Warn("Attempt to identify an already identified assembly in the plugin");
			}

			// used to dispose unnecessary assemblies
			bool initializedPlugin = false;

			foreach (Type type in assembly.GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(Plugin))))
			{
				try
				{
					Plugin plugin = (Plugin)Activator.CreateInstance(type);
					PluginDefineAttribute definer = assembly.GetCustomAttribute<PluginDefineAttribute>();

					if (definer == null)
					{
						Warn($"Attempt to initialize the plugin without PluginDefine attribute: {assembly.FullName}");
						continue;
					}

					if (definer.Id != null)
					{
						if (IsNotUniqueId(definer.Id))
						{
							Warn($"Several plugins try to use the same id: {definer.Id}");
							continue;
						}

						if (definer.SmodMajor != SMOD_MAJOR && definer.SmodMinor != SMOD_MINOR)
						{
							Logger.Warn("PLUGIN_LOADER", $"Initializing an outdated plugin: {plugin}");
						}


						plugin.Definer = definer;
						plugin.Pipes = new PluginPipes(plugin);

						plugin.Status = PluginStatus.DISABLED;

						try
						{
							plugin.CallAwake();
						}
						catch (Exception e)
						{
							Error("Registering the plugin throw an exception");
							Debug(e.Message);
							Debug(e.StackTrace);
						}

						Logger.Info("PLUGIN_LOADER", $"Plugin loaded: {plugin}");

						pluginAssemblies.Add(plugin.Definer.Id, assembly);

						initializedPlugin = true;
					}
					else
					{
						Logger.Warn("PLUGIN_LOADER", $"The plugin was not loaded, it has a missing id: {type} [{assembly}]");
					}
				}
				catch (Exception e)
				{
					Error($"Failed to create instance of plugin: {type} [{assembly}]");
					Error($"{e.GetType().Name}: {e.Message}");
					Error(e.StackTrace);
				}
			}

			if (!initializedPlugin)
			{
				assembly = null;
			}
		}

		public void ReloadPlugins()
		{
			for (int z = 0; z < plugins.Count; z++)
			{
				DestroyPlugin(plugins[z]);
			}

			LoadPlugins();
		}

		public void ReloadDependencies()
		{
			for (int z = 0; z < dependencies.Count; z++)
			{
				dependencies[z] = null;
			}
			dependencies.Clear();

			LoadDependencies();
		}

		// with toDot = _myLove_warialbe => my.love.warialbe
		// without toDot = _myLove_warialbe => my_love_warialbe
		public static string ToLowerSnakeCase(string otherCase, bool toDot = false)
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < otherCase.Length; i++)
			{
				char curCase = otherCase[i];
				if (builder.Length == 0 && curCase == '_')
				{
					continue;
				}

				if (i > 0 && char.IsUpper(curCase) && otherCase[i - 1] != '_')
				{
					builder.Append(string.Concat(toDot ? "." : "_", char.ToLower(curCase)));
				}
				else
				{
					if (toDot && curCase == '_')
					{
						curCase = '.';
					}

					builder.Append(char.ToLower(curCase));
				}
			}

			return builder.ToString();
		}
	}
}
