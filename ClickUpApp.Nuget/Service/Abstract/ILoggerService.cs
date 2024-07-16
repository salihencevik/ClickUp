
namespace ClickUpApp.Nuget.Service
{
	public interface ILoggerService
	{
		void LogInfo(Exception ex);
		void LogWarn(Exception ex);
		void LogDebug(Exception ex);
		void LogError(Exception ex);
		void LogTrace(Exception ex);
	}
}
