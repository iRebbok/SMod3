using SMod3.Core;
using SMod3.Module.Attributes.Meta;
using SMod3.Module.Piping.Meta;
using System;
using System.Linq;
using System.Reflection;

namespace SMod3.Module.Piping.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class PipeEventAttribute : BaseAttribute
    {
        public string[] PluginScope { get; }
        public string EventName { get; }

        public PipeEventAttribute(string eventName) : this(eventName, null) { }
        public PipeEventAttribute(string eventName, params string[] pluginScope)
        {
            EventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
            PluginScope = pluginScope ?? new string[0];
        }
    }

    public sealed class EventPipe : MemberPipe
    {
        private readonly MethodInfo info;
        private string[] pluginScope;

        public string EventName { get; }

        internal EventPipe(Plugin source, MethodInfo info, PipeEventAttribute pipe) : base(source, info, info.IsStatic)
        {
            EventName = pipe.EventName;
            pluginScope = pipe.PluginScope;

            this.info = info;
            Type = typeof(void);

            //if (info.ReturnType != Type)
            //{
            //    PluginManager.Manager.Logger.Warn("PIPE_MANAGER", $"Pipe event {source} returns a value. This is bad practice.");
            //}
        }

        public string[] GetPluginScope() => pluginScope.ToArray();

        private void LogException(Exception e, string caller, string possibleError)
        {
            Source.Error($"Failed to handle event pipe: {EventName} (method: {Name}, caller: {caller ?? "Unspecified"}). Possible error: {possibleError}.");
            Source.Error(e.GetType().Name + ": " + e.Message);
            Source.Error(e.StackTrace);
        }

        private void LogException(Exception e, string caller)
        {
            Source.Error($"Failed to handle event pipe: {EventName} (method: {Name}, caller: {caller ?? "Unspecified"})");
            Source.Error(e.GetType().Name + ": " + e.Message);
            Source.Error(e.StackTrace);
        }

        internal void Invoke(object[] parameters, string caller)
        {
            try
            {
                info.Invoke(Source, parameters);
            }
            catch (TargetParameterCountException e)
            {
                if (e.TargetSite.Name != "ConvertValues")
                {
                    throw;
                }

                LogException(e, caller, "invalid number of parameters");
            }
            catch (ArgumentException e)
            {
                if (e.TargetSite.Name != "CheckValue")
                {
                    throw;
                }

                LogException(e, caller, "invalid type of parameters");
            }
            catch (Exception e)
            {
                LogException(e, caller);
            }
        }
    }
}
