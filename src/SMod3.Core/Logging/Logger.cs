using System;

namespace SMod3.Core.Logging
{
    public static class Logger
    {
        /// <summary>
        ///     Fires when a new log is received.
        /// </summary>
        public static event Action<LogMessage>? NewLog;

        /// <summary>
        ///     Logging under the <see cref="LogSensitivity.CRITICAL"/> level.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     The message is null, empty or whitespace.
        /// </exception>
        public static void Critical(string? tag, string message)
        {
            Raw(LogSensitivity.CRITICAL, tag, message);
        }

        /// <summary>
        ///     Logging under the <see cref="LogSensitivity.ERROR"/> level.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     The message is null, empty or whitespace.
        /// </exception>
        public static void Error(string? tag, string message)
        {
            Raw(LogSensitivity.ERROR, tag, message);
        }

        /// <summary>
        ///     Logging under the <see cref="LogSensitivity.WARN"/> level.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     The message is null, empty or whitespace.
        /// </exception>
        public static void Warn(string? tag, string message)
        {
            Raw(LogSensitivity.WARN, tag, message);
        }

        /// <summary>
        ///     Logging under the <see cref="LogSensitivity.INFO"/> level.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     The message is null, empty or whitespace.
        /// </exception>
        public static void Info(string? tag, string message)
        {
            Raw(LogSensitivity.INFO, tag, message);
        }

        /// <summary>
        ///     Logging under the <see cref="LogSensitivity.VERBOSE"/> level.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     The message is null, empty or whitespace.
        /// </exception>
        public static void Verbose(string? tag, string message)
        {
            Raw(LogSensitivity.VERBOSE, tag, message);
        }

        /// <summary>
        ///     Logging under the <see cref="LogSensitivity.DEBUG"/> level.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     The message is null, empty or whitespace.
        /// </exception>
        public static void Debug(string? tag, string message)
        {
            Raw(LogSensitivity.DEBUG, tag, message);
        }

        /// <summary>
        ///     Raw log processing.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     The message is null, empty or whitespace.
        /// </exception>
        public static void Raw(LogSensitivity sensitivity, string? tag, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message cannot be null, empty or whitespace", nameof(message));

            // This should never happen,
            // the main and first listener is SMBootstrapper,
            // but still check.
            if (NewLog is null)
                return;

            var logMessage = new LogMessage(sensitivity, tag, message);
            SendLog(logMessage);
        }

        /// <summary>
        ///     Sends a log directly.
        /// </summary>
        public static void SendLog(LogMessage logMessage)
        {
            // The final validation
            if (NewLog is null)
                return;

            NewLog.Invoke(logMessage);
        }
    }
}
