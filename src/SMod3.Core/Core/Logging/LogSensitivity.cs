namespace SMod3.Core.Logging
{
    public enum LogSensitivity
    {
        /// <summary>
        ///     A type of log that requires immediate user intervention.
        /// </summary>
        CRITICAL = 0,
        /// <summary>
        ///     A type of log that requires attention.
        /// </summary>
        ERROR = 1,
        /// <summary>
        ///     A type of log that needs to be fixed by the user, but doesn't violate the runtime.
        /// </summary>
        WARN = 2,
        /// <summary>
        ///     A type of log that informs the user about the general flow.
        /// </summary>
        INFO = 3,
        /// <summary>
        ///     A type of log that shows what is happening.
        /// </summary>
        /// <remarks>
        ///     The difference from <see cref="DEBUG"/> is that there is less detailed information about what is happening,
        ///     only an exception message.
        ///
        ///     <para>
        ///         ProTip: use this type of log only for exceptions and errors that will NOT affect the main workflow that might be hidden.
        ///     </para>
        /// </remarks>
        VERBOSE = 4,
        /// <summary>
        ///     A type of log that shows the details of what is happening.
        /// </summary>
        /// <remarks>
        ///     The difference from <see cref="VERBOSE"/> is that has more detailed information about what is happening,
        ///     for example, a full StackTrace of exception.
        ///
        ///     <para>
        ///         ProTip: use this type of log only for exceptions and errors that will NOT affect the main workflow that might be hidden.
        ///     </para>
        /// </remarks>
        DEBUG = 5
    }
}
