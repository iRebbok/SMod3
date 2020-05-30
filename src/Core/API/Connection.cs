namespace SMod3.Core.API
{
    public abstract class Connection
    {
        public abstract string IpAddress { get; }
        public abstract void Disconnect();
        public abstract bool IsBanned { get; }
    }
}
