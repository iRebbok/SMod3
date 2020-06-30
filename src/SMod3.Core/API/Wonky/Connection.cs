namespace SMod3.API
{
    public abstract class Connection
    {
        public abstract string IpAddress { get; }
        public abstract void Disconnect();
    }
}
