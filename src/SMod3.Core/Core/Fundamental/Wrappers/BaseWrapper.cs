using System;
using System.Reflection;

namespace SMod3.Core.Fundamental
{
    /// <remarks>
    ///     Since the event system provides the ability
    ///     to register events not exclusively on the inherited interfaces,
    ///     we provide Assembly as the owner of the wrapper.
    /// </remarks>
    public abstract class BaseWrapper
    {
        /// <summary>
        ///		True owner of this wrapper.
        /// </summary>
        public Assembly Owner { get; }

        /// <summary>
        ///     Part-owner of this wrapper.
        /// </summary>
        public Plugin? PartOwner { get; }

        protected BaseWrapper(Plugin partOwner)
        {
            if (partOwner is null)
                throw new ArgumentNullException(nameof(partOwner));

            Owner = partOwner.GetType().Assembly;
            PartOwner = partOwner;
        }

        protected BaseWrapper(Assembly owner)
        {
            if (owner is null)
                throw new ArgumentNullException(nameof(owner));

            Owner = owner;
            PartOwner = null;
        }
    }
}
