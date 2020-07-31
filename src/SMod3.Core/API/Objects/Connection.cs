namespace SMod3.API
{
    public abstract class Connection
    {
        /// <summary>
        ///     Gets the IP address without a port.
        /// </summary>
        public abstract string IpAddress { get; }

        /// <summary>
        ///     Gets the ping.
        /// </summary>
        public abstract int Ping { get; }

        public abstract void Disconnect();
    }
}
