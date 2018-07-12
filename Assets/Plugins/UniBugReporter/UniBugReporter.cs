using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace UniBugReporter
{
	public class UniBugReporter : MonoBehaviour
	{
		[SerializeField]
		public bool EnableOnEditor;

		public static string ScreenShotFileName { get; set; } /* = "BugReport.png"; */
		public static string ScreenShotFilePath { get { return Path.Combine(Application.persistentDataPath, ScreenShotFileName); } }
		public static IReporter Reporter = new NothingReporter();

		public void Awake()
		{
			AwakeBody();
		}

		public void OnEnable()
		{
			OnEnableBody();
		}

		public void OnDisable()
		{
			OnDisableBody();
		}

		[Conditional("DEVELOPMENT_BUILD")]
		private void AwakeBody()
		{
			DontDestroyOnLoad(gameObject);
			ScreenShotFileName = "BugReport.png";
		}

		[Conditional("DEVELOPMENT_BUILD")]
		private void OnEnableBody()
		{
			Application.logMessageReceived += HandleLog;
		}

		[Conditional("DEVELOPMENT_BUILD")]
		private void OnDisableBody()
		{
			Application.logMessageReceived -= HandleLog;
		}

		private void HandleLog(string message, string stackTrace, LogType type)
		{
			if (string.IsNullOrEmpty(message)) return;
			if (!EnableOnEditor && Application.isEditor) return;
			if (!Reporter.IsReportTarget(message, stackTrace, type)) return;

			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				StartCoroutine(CaptureScreenshot(() =>
				{
					Reporter.Report(message, stackTrace, type, ScreenShotFilePath);
				}));
			}
			else
			{
				Reporter.Report(message, stackTrace, type, null);
			}
		}

		private IEnumerator CaptureScreenshot(Action callback)
		{
			Directory.CreateDirectory(Application.persistentDataPath);
			if (File.Exists(ScreenShotFilePath)) File.Delete(ScreenShotFilePath);
			ScreenCapture.CaptureScreenshot(ScreenShotFileName);
			yield return new WaitUntil(() => File.Exists(ScreenShotFilePath));
			callback();
		}

		[Flags]
		public enum UnityLogType
		{
			Exception = (1 << 0),
			Error = (1 << 1),
			Assert = (1 << 2),
			Warning = (1 << 3),
			Log = (1 << 4),
		}
	}
}
