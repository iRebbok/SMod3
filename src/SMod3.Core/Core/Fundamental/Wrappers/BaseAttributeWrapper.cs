using System;
using System.Reflection;

namespace SMod3.Core.Fundamental
{
    /// <summary>
    ///		Wrapper that stores entities for implementing interaction with attributes.
    ///		Used by all modules that implement interaction via attributes.
    /// </summary>
    public class BaseAttributeWrapper : BaseWrapper
    {
        public enum TargetType
        {
            Method = 0,
            Field = 1,
            Property = 2
        }

        /// <summary>
        ///		Instance of the object that the wrapper owner.
        /// </summary>
        public object? Instance { get; }

        /// <summary>
        ///		Variable that the attribute points to.
        /// </summary>
        public MemberInfo Target { get; }

        public TargetType Type { get; }

        public BaseAttributeWrapper(Plugin partOwner, object? instance, MemberInfo target) : base(partOwner)
        {
            Instance = instance;
            Target = target;
            Type = FindType(target);
        }

        public BaseAttributeWrapper(Assembly owner, object? instance, MemberInfo target) : base(owner)
        {
            Instance = instance;
            Target = target;
            Type = FindType(target);
        }

        private static TargetType FindType(MemberInfo target)
        {
            if (target is MethodInfo)
                return TargetType.Method;

            if (target is FieldInfo)
                return TargetType.Field;

            if (target is PropertyInfo)
                return TargetType.Property;

            throw new NotSupportedException("Wrap target type cannot be determined");
        }
    }
}
