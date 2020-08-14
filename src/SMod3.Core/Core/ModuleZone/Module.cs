using SMod3.Core.Fundamental;

namespace SMod3.Core
{
    public abstract class Module : BaseManager
    {
#nullable disable
        public ModuleMetadata Metadata { get; internal set; }
#nullable restore

        /// <summary>
        ///     Module starting point.
        /// </summary>
        protected virtual void Awake() { }

        internal void CallAwake() => Awake();

        public override string ToString()
        {
            if (Metadata is null)
                return base.ToString();

            return $"{Metadata.Id} ({Metadata.Version})";
        }
    }
}
