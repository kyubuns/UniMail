using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UniMail;

namespace UniMail.Sample
{
	public class Sample : MonoBehaviour
	{
		private static readonly string ScreenShotFileName = "ScreenShot.png";
		private static string ScreenShotPath
		{
			get
			{
				if (Application.platform == RuntimePlatform.IPhonePlayer) return Path.Combine(Application.persistentDataPath, ScreenShotFileName);
				return ScreenShotFileName;
			}
		}

		public void SendEmail()
		{
			Mail.Send("unimail@example.com", "subject", "body1\nbody2");
		}

		public void SendEmailWithScreenShot()
		{
			StartCoroutine(CaptureScreenShot(() =>
			{
				Mail.SendWithImage("unimail@example.com", "subject", "body1\nbody2", ScreenShotPath);
			}));
		}

		private IEnumerator CaptureScreenShot(Action callback)
		{
			Directory.CreateDirectory(Application.persistentDataPath);
			if (File.Exists(ScreenShotPath)) File.Delete(ScreenShotPath);
			ScreenCapture.CaptureScreenshot(ScreenShotFileName);

			yield return new WaitUntil(() => File.Exists(ScreenShotPath));
			callback();
		}
	}
}