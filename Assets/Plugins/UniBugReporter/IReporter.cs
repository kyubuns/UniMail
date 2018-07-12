using UnityEngine;

namespace UniBugReporter
{
	public interface IReporter
	{
		bool IsReportTarget(string message, string stackTrace, LogType type);
		void Report(string message, string stackTrace, LogType type, string screenshotFilePath);
	}

	public class NothingReporter : IReporter
	{
		public bool IsReportTarget(string message, string stackTrace, LogType type)
		{
			return false;
		}

		public void Report(string message, string stackTrace, LogType type, string screenshotFilePath)
		{
		}
	}
}