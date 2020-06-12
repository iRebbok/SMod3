using System;
using System.Reflection;

using SMod3.Core;
using SMod3.Module.Piping.Meta;

namespace SMod3.Module.Piping.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class PipePropertyAttribute : Attribute
    {
        public bool Gettable { get; private set; }
        public bool Settable { get; private set; }

        public PipePropertyAttribute() : this(true, true) { }
        public PipePropertyAttribute(bool gettable, bool settable)
        {
            Gettable = gettable;
            Settable = settable;
        }
    }

    public class PropertyPipe : MemberPipe
    {
        private PropertyInfo info;
        public object Value
        {
            get
            {
                if (!Gettable)
                {
                    throw new InvalidOperationException($"Cannot get ungettable property pipe: {(info.DeclaringType == null ? info.Name : info.DeclaringType.FullName + "." + info.Name)}");
                }

                return info.GetValue(Source);
            }
            set
            {
                if (!Settable)
                {
                    throw new InvalidOperationException($"Cannot set unsettable property pipe: {(info.DeclaringType == null ? info.Name : info.DeclaringType.FullName + "." + info.Name)}");
                }

                info.SetValue(Source, value);
            }
        }
        public bool Gettable { get; }
        public bool Settable { get; }
        internal PropertyPipe(Plugin source, PropertyInfo info, PipePropertyAttribute pipe) : base(source, info, info.GetMethod?.IsStatic ?? info.SetMethod.IsStatic)
        {
            Gettable = pipe.Gettable;
            Settable = pipe.Settable;

            this.info = info;

            Type = info.PropertyType;
            Gettable = Gettable && info.GetMethod != null;
            Settable = Settable && info.SetMethod != null;

            //if (!Gettable && !Settable)
            //{
            //    PluginManager.Manager.Logger.Warn("PIPE_MANAGER", $"Pipe property {Name} in {Source.Definer.Id} has no accessable getter or setter. This is bad practice.");
            //}
        }
    }

    public sealed class PropertyPipe<T> : PropertyPipe
    {
        public new T Value
        {
            get => (T)base.Value;
            set => base.Value = value;
        }
        internal PropertyPipe(Plugin source, PropertyInfo info, PipePropertyAttribute pipe) : base(source, info, pipe) { }

        public static implicit operator T(PropertyPipe<T> pipe) => pipe.Value;
    }
}
