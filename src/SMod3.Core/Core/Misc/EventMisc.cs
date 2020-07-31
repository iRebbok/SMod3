using System;

using SMod3.Core.Logging;

namespace SMod3.Core.Misc
{
    public static class EventMisc
    {
        public static readonly string LoggingTag = StringMisc.ToFullyUpperSnakeCase(nameof(EventMisc));

        public static void InvokeSafely<T>(CustomDelegate<T>? @delegate, T arg)
        {
            if (@delegate is null)
                return;

            var eventName = @delegate.GetType().FullName;
            foreach (CustomDelegate<T> subscriber in @delegate.GetInvocationList())
            {
                try
                { subscriber(arg); }
                catch (Exception ex)
                { LogException(ex, eventName); }
            }
        }

        public static void InvokeSafely(CustomDelegate? @delegate)
        {
            if (@delegate is null)
                return;

            var eventName = @delegate.GetType().FullName;
            foreach (CustomDelegate subscriber in @delegate.GetInvocationList())
            {
                try
                { subscriber(); }
                catch (Exception ex)
                { LogException(ex, eventName); }
            }
        }

        private static void LogException(Exception ex, string eventName)
        {
            Logger.Error(LoggingTag, $"Exception while handling event '{eventName}'");
            Logger.Debug(LoggingTag, ex.ToString());
        }
    }
}
