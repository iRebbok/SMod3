using SMod3.Core.Logging;

namespace SMod3.Core.Meta
{
	public abstract class BaseLogging
	{
		/// <summary>
		///		Used to identify the logging tag.
		/// </summary>
		public abstract string LoggingTag { get; }

		public virtual void Debug(string message)
		{
			Logger.Singleton.Debug(LoggingTag, message);
		}

		public virtual void Info(string message)
		{
			Logger.Singleton.Info(LoggingTag, message);
		}

		public virtual void Warn(string message)
		{
			Logger.Singleton.Warn(LoggingTag, message);
		}

		public virtual void Error(string message)
		{
			Logger.Singleton.Error(LoggingTag, message);
		}
	}
}
