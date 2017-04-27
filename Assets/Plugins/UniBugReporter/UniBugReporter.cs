using System;
using System.IO;
using System.Collections;
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
			DontDestroyOnLoad(gameObject);
			ScreenShotFileName = "BugReport.png";
		}

		public void OnEnable()
		{
			Application.logMessageReceived += HandleLog;
		}

		public void OnDisable()
		{
			Application.logMessageReceived -= HandleLog;
		}

		private void HandleLog(string message, string stackTrace, LogType type)
		{
			if (string.IsNullOrEmpty(message)) return;
			if (!EnableOnEditor && Application.isEditor) return;

			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				StartCoroutine(CaptureScreenshot(() =>
				{
					Reporter.Report(message, stackTrace, type, null);
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
			Application.CaptureScreenshot(ScreenShotFileName);
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