namespace SMod3.API
{
    public abstract class Scp268PlayerEffect : BasePlayerEffect
    {
        /// <summary>
        ///     Current usage time.
        /// </summary>
        public abstract float CurrentTime { get; set; }

        /// <summary>
        ///     Usage time, if set to null, it'll be infinite.
        /// </summary>
        public abstract float? UsingTime { get; set; }
    }
}
