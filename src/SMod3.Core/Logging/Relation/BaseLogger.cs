namespace SMod3.Core.Logging
{
    /// <summary>
    ///     Basic logger implementation.
    /// </summary>
    public abstract class BaseLogger : ILogger
    {
        public abstract string? LoggingTag { get; }

        public virtual void Critical(string message)
        {
            Logger.Critical(LoggingTag, message);
        }

        public virtual void Error(string message)
        {
            Logger.Error(LoggingTag, message);
        }

        public virtual void Warn(string message)
        {
            Logger.Warn(LoggingTag, message);
        }

        public virtual void Info(string message)
        {
            Logger.Info(LoggingTag, message);
        }

        public void Verbose(string message)
        {
            Logger.Verbose(LoggingTag, message);
        }

        public void Debug(string message)
        {
            Logger.Debug(LoggingTag, message);
        }
    }
}
