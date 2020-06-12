using System;
using System.Reflection;

using SMod3.Core.Fundamental;

namespace SMod3.Core.Imbedded.Attribute
{
    /// <summary>
    ///		Wrapper that stores entities for implementing interaction with attributes.
    ///		Used by all modules that implement interaction via attributes.
    /// </summary>
    public class BaseAttributeWrapper : BaseWrapper
    {
        /// <summary>
        ///		Instance of the object whose attribute value is setting.
        ///		It can be null if the field is static!
        /// </summary>
        public object Instance { get; }
        /// <summary>
        ///		Variable that the attribute points to.
        /// </summary>
        public FieldInfo Field { get; }

        public BaseAttributeWrapper(Plugin owner, object instance, FieldInfo field) : base(owner)
        {
            Instance = instance;
            Field = field ?? throw new ArgumentNullException(nameof(field));
        }
    }
}
