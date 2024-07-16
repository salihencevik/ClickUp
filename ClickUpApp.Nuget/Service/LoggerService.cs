using NLog;
using ClickUpApp.Nuget.Service;

namespace ClickUpApp.Nuget
{
	public class LoggerService : ILoggerService
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();
		public LoggerService()
		{
		}
		public void LogDebug(Exception ex)
		{
			logger.Debug(ex);
		}

		public void LogError(Exception ex)
		{
			logger.Error(ex);
		}

		public void LogInfo(Exception ex)
		{
			logger.Info(ex);
		}

		public void LogWarn(Exception ex)
		{
			logger.Warn(ex);
		}
		public void LogTrace(Exception ex)
		{
			logger.Trace(ex);
		}
	}
}
