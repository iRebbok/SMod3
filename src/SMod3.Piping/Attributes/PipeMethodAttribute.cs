using System;
using System.Linq;
using System.Reflection;

using SMod3.Core;
using SMod3.Module.Attributes.Meta;
using SMod3.Module.Piping.Meta;

namespace SMod3.Module.Piping.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class PipeMethodAttribute : BaseAttribute { }

    public class MethodPipe : MemberPipe
    {
        private readonly MethodInfo info;
        private readonly MethodPipeParameter[] pipeParameters;
        public object Invoke() => Invoke(null);
        public object Invoke(params object[] parameters)
        {
            try
            {
                return info.Invoke(Source, parameters);
            }
            catch (TargetParameterCountException e)
            {
                if (e.TargetSite.Name != "ConvertValues")
                {
                    throw;
                }

                throw new TargetParameterCountException($"{pipeParameters.Length} parameters were expected, but {parameters?.Length ?? 0} were supplied.", e);
            }
            catch (ArgumentException e)
            {
                if (e.TargetSite.Name != "CheckValue")
                {
                    throw;
                }

                throw new ArgumentException("Parameter type mismatch. Check the plugin which hosts the method pipe and make sure your arguments types are correct.", e);
            }
        }

        internal MethodPipe(Plugin source, MethodInfo info) : base(source, info, info.IsStatic)
        {
            this.info = info;

            Type = info.ReturnType;
            ParameterInfo[] parameters = info.GetParameters();
            pipeParameters = new MethodPipeParameter[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                pipeParameters[i] = new MethodPipeParameter(parameters[i]);
            }

            //if (!info.IsPublic)
            //{
            //    PluginManager.Manager.Logger.Warn("PIPE_MANAGER", $"Pipe method {Name} in {Source.Definer.Id} is not public. This is bad practice.");
            //}
        }

        public MethodPipeParameter[] GetParameters() => pipeParameters.ToArray();
    }
    public sealed class MethodPipe<T> : MethodPipe
    {
        public new T Invoke() => Invoke(null);
        public new T Invoke(params object[] parameters) => (T)base.Invoke(parameters);
        internal MethodPipe(Plugin source, MethodInfo info) : base(source, info) { }
    }
}
