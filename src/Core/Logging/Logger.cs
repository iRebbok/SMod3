namespace SMod3.Core.Logging
{
    public sealed class Logger
    {
        private Logger() { }

        public static Logger Singleton { get; } = new Logger();

        public delegate void OnLog(string level, string tag, string message);
        public static event OnLog OnLogEvent;

        public void Debug(string tag, string message)
        {
            OnLogEvent.Invoke("DEBUG", tag, message);
        }

        public void Info(string tag, string message)
        {
            OnLogEvent.Invoke("INFO", tag, message);
        }

        public void Warn(string tag, string message)
        {
            OnLogEvent.Invoke("WARN", tag, message);
        }

        public void Error(string tag, string message)
        {
            OnLogEvent.Invoke("EERROR", tag, message);
        }

        public void Raw(string message)
        {
            OnLogEvent.Invoke(null, null, message);
        }
    }
}
