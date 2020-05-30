using SMod3.Core;
using SMod3.Module.Attributes.Meta;
using SMod3.Module.Piping.Meta;
using System;
using System.Reflection;

namespace SMod3.Module.Piping.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class PipeFieldAttribute : BaseAttribute
    {
        public bool Readonly { get; }

        public PipeFieldAttribute() : this(false) { }
        public PipeFieldAttribute(bool @readonly)
        {
            Readonly = @readonly;
        }
    }

    public class FieldPipe : MemberPipe
    {
        protected readonly FieldInfo info;
        public object Value
        {
            get => info.GetValue(instance);
            set
            {
                if (Readonly)
                {
                    throw new InvalidOperationException($"Cannot set readonly field pipe: {(info.DeclaringType == null ? info.Name : info.DeclaringType.FullName + "." + info.Name)}");
                }

                info.SetValue(instance, value);
            }
        }

        public bool Readonly { get; }

        internal FieldPipe(Plugin source, FieldInfo info, PipeFieldAttribute pipe) : base(source, info, info.IsStatic)
        {
            Readonly = pipe.Readonly;

            this.info = info;

            Type = info.FieldType;
            Readonly = Readonly && info.IsInitOnly;

            if (!info.IsPublic)
            {
                PluginManager.Manager.Logger.Warn("PIPE_MANAGER", $"Pipe field {Name} in {Source.Definer.Id} is not public. This is bad practice.");
            }
        }
    }

    public sealed class FieldPipe<T> : FieldPipe
    {
        public new T Value
        {
            get => (T)base.Value;
            set => base.Value = value;
        }

        internal FieldPipe(Plugin source, FieldInfo info, PipeFieldAttribute pipe) : base(source, info, pipe) { }

        public static implicit operator T(FieldPipe<T> pipe) => pipe.Value;
    }
}
