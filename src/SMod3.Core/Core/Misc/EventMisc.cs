using System;
using System.Collections.Generic;
using System.Reflection;

using SMod3.Core.Logging;

namespace SMod3.Core.Misc
{
    /// <remarks>
    ///     This class is not public because it's not finished yet.
    ///     I have an idea to start events in a circle until they stop throwing exceptions,
    ///     cutting off every time those who throw an exception. But s00n.
    /// </remarks>
    internal static class EventMisc
    {
        private static readonly string loggingTag = StringMisc.ToFullyUpperSnakeCase(nameof(EventMisc));

        internal static void InvokeSafely(MulticastDelegate? multicastDelegate, params object[] args)
        {
            if (multicastDelegate is null)
                return;

            var eventName = multicastDelegate.GetType().FullName;
            InvokeSafely(eventName, multicastDelegate.GetInvocationList(), args);
        }

        /// <exception cref="ArgumentNullException">
        ///     Nullable delegates passed.
        /// </exception>
        internal static void InvokeSafely(string eventName, IEnumerable<Delegate> delegates, params object[] args)
        {
            if (delegates is null)
                throw new ArgumentNullException("Event cannot be null", nameof(delegates));

            foreach (var @delegate in delegates)
                HandleSafely(eventName, @delegate.Method, @delegate.Target, args);
        }

        private static void HandleSafely(string eventName, MethodInfo methodInfo, object target, params object[] args)
        {
            try { methodInfo.Invoke(target, args); }
            catch (Exception ex)
            {
                try
                {
                    Logger.Error(loggingTag, $"Method '{methodInfo.DeclaringType.FullName}.{methodInfo.Name}' threw an exception while processing the event '{eventName}': {ex.Message}");
                    Logger.Debug(loggingTag, ex.ToString());
                }
                catch { }
            }
        }
    }
}
