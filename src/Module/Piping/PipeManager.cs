using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using SMod3.Core;
using SMod3.Core.Meta;
using SMod3.Module.Attributes;
using SMod3.Module.Piping.Attributes;
using SMod3.Module.Piping.Meta;

namespace SMod3.Module.Piping
{
	public sealed class PipeManager : BaseManager
	{
		public static PipeManager Manager { get; } = new PipeManager();

		private PipeManager() { }

		public override string LoggingTag => "PIPE_MANAGER";

		private readonly Dictionary<Plugin, List<FieldInfo>> linkFields = new Dictionary<Plugin, List<FieldInfo>>();
		private readonly Dictionary<Plugin, Dictionary<Plugin, List<FieldInfo>>> linkFieldReferences = new Dictionary<Plugin, Dictionary<Plugin, List<FieldInfo>>>();

		private readonly Dictionary<string, List<EventPipe>> events = new Dictionary<string, List<EventPipe>>();
		private readonly Dictionary<Type, Func<PluginPipes, string, object>> pipeGetters = new Dictionary<Type, Func<PluginPipes, string, object>>
		{
			[typeof(FieldPipe)] = (pipes, pipeName) => pipes.GetField(pipeName),
			[typeof(PropertyPipe)] = (pipes, pipeName) => pipes.GetProperty(pipeName),
			[typeof(MemberPipe)] = (pipes, pipeName) => pipes.GetMethod(pipeName)
		};

		private void SetPipeLink(Plugin source, FieldInfo info, string pluginId, string pipeName)
		{
			Plugin target = PluginManager.Manager.GetPlugin(pluginId);

			Type fieldType = info.FieldType;
			if (!pipeGetters.ContainsKey(fieldType))
			{
				// Set type of field to the base (non-generic type) if the generic is a MemberPipe
				if (typeof(MemberPipe).IsAssignableFrom(fieldType))
				{
					fieldType = fieldType.BaseType;
				}

				if (fieldType == null || !pipeGetters.ContainsKey(fieldType))
				{
					PluginManager.Manager.Logger.Error("PIPE_MANAGER", $"{info.Name} of {source.Definer.Id} tried to link to a non-existant pipe type: {info.FieldType}");
					return;
				}
			}

			if (!linkFieldReferences.ContainsKey(target))
			{
				linkFieldReferences.Add(target, new Dictionary<Plugin, List<FieldInfo>>());
			}
			Dictionary<Plugin, List<FieldInfo>> references = linkFieldReferences[target];

			if (!references.ContainsKey(source))
			{
				references.Add(source, new List<FieldInfo>());
			}
			references[source].Add(info);

			info.SetValue(source, Convert.ChangeType(pipeGetters[fieldType].Invoke(target.Pipes, pipeName), info.FieldType));
		}

		public override void Dispose(Plugin plugin)
		{
			if (plugin == null) return;

			foreach (EventPipe pipe in plugin.Pipes.GetEvents())
			{
				if (events.ContainsKey(pipe.EventName))
				{
					events[pipe.EventName].Remove(pipe);
				}
			}

			UnregisterLinks(plugin);
		}

		public List<string> GetLinkDependencies(Plugin plugin)
		{
			Type type = plugin.GetType();
			List<string> dependencies = new List<string>();

			foreach (MemberInfo info in type.GetMembers(PluginManager.ALL_BINDING_FLAGS))
			{
				PipeLinkAttribute link = info.GetCustomAttribute<PipeLinkAttribute>();
				if (!dependencies.Contains(link.Plugin))
				{
					dependencies.Add(link.Plugin);
				}
			}

			return dependencies;
		}

		public void RegisterAttributes<TInstance>(Plugin plugin, TInstance instance)
		{
			if (plugin == null || instance == null) return;

			var attributes = AttributeManager.Manager.PullAttributes<PipeLinkAttribute, TInstance>(instance);

			List<FieldInfo> fields = new List<FieldInfo>();
			foreach (var pair in attributes)
			{
				Debug($"Linking {pair.Value.Name} of {plugin.Definer.Id} to {pair.Key.Pipe} of {pair.Key.Plugin}.");
				SetPipeLink(plugin, pair.Value, pair.Key.Plugin, pair.Key.Pipe);
			}

			linkFields.Add(plugin, fields);
		}

		public void UnregisterLinks(Plugin plugin)
		{
			foreach (FieldInfo info in linkFields[plugin])
			{
				PipeLinkAttribute link = info.GetCustomAttribute<PipeLinkAttribute>();
				if (link != null)
				{
					PluginManager.Manager.Logger.Debug("PIPE_MANAGER", $"Unlinking {info.Name} of {plugin.Definer.Id} from {link.Pipe} of {link.Plugin}");
					info.SetValue(plugin, null);
				}
			}
			linkFields.Remove(plugin);

			if (linkFieldReferences.ContainsKey(plugin))
			{
				PluginManager.Manager.Logger.Debug("PIPE_MANAGER", $"Unlinking all field references to {plugin.Definer.Id}");

				foreach (KeyValuePair<Plugin, List<FieldInfo>> infos in linkFieldReferences[plugin])
				{
					foreach (FieldInfo info in infos.Value)
					{
						PipeLinkAttribute link = info.GetCustomAttribute<PipeLinkAttribute>();
						if (link != null)
						{
							info.SetValue(infos.Key, null);
						}
					}
				}
			}
			linkFieldReferences.Remove(plugin);
		}

		internal void InvokeEvent(string eventName, string caller, object[] parameters)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException(nameof(eventName));
			}

			if (!events.ContainsKey(eventName))
			{
				return;
			}

			foreach (EventPipe pipe in events[eventName])
			{
				// Skip if event pipe is disabled
				if (PluginManager.Manager.GetDisabledPlugin(pipe.Source.Definer.Id) != null)
				{
					continue;
				}

				// Skip if event pipe is specific to certain plugins AND the scope does not contain the invoker
				string[] pluginScope = pipe.GetPluginScope();
				if (pluginScope.Length > 0 && !pluginScope.Contains(caller))
				{
					continue;
				}

				pipe.Invoke(parameters, caller);
			}
		}
	}
}
