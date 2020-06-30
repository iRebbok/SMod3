namespace SMod3.Core.Logging
{
    public interface ILogger
    {
        /// <summary>
        ///		Used to identify the logging tag.
        /// </summary>
        abstract string? LoggingTag { get; }


        /// <summary>
        ///     Logging under the <see cref="LogSensitivity.CRITICAL"/> level.
        /// </summary>
        void Critical(string message);
        /// <summary>
        ///     Logging under the <see cref="LogSensitivity.ERROR"/> level.
        /// </summary>
        void Error(string message);
        /// <summary>
        ///     Logging under the <see cref="LogSensitivity.WARN"/> level.
        /// </summary>
        void Warn(string message);
        /// <summary>
        ///     Logging under the <see cref="LogSensitivity.INFO"/> level.
        /// </summary>
        void Info(string message);
        /// <summary>
        ///     Logging under the <see cref="LogSensitivity.VERBOSE"/> level.
        /// </summary>
        void Verbose(string message);
        /// <summary>
        ///     Logging under the <see cref="LogSensitivity.DEBUG"/> level.
        /// </summary>
        void Debug(string message);
    }
}
