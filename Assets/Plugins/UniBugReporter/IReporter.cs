using UnityEngine;

namespace UniBugReporter
{
	public interface IReporter
	{
		void Report(string message, string stackTrace, LogType type, string screenshotFilePath);
	}

	public class NothingReporter : IReporter
	{
		public void Report(string message, string stackTrace, LogType type, string screenshotFilePath)
		{
		}
	}
}