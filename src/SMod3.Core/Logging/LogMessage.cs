namespace SMod3.Core.Logging
{
    /// <summary>
    ///     Intermediate log message.
    /// </summary>
    public readonly struct LogMessage
    {
        public LogSensitivity Sensitivity { get; }

        public string? Tag { get; }

        public string Message { get; }

        public LogMessage(LogSensitivity sensitivity, string? tag, string message)
        {
            Sensitivity = sensitivity;
            Tag = tag;
            Message = message;
        }
    }
}
