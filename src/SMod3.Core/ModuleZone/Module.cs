using SMod3.Core.Fundamental;

namespace SMod3.Core
{
    public abstract class Module : BaseManager
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public ModuleMetadata Metadata { get; internal set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        /// <summary>
        ///     Module starting point.
        /// </summary>
        protected virtual void Awake() { }

        internal void CallAwake() => Awake();
    }
}
