using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UniMail
{
	public static class Mail
	{
		public static readonly string Version = "0.9.0";

#if UNITY_IOS && !UNITY_EDITOR
		[DllImport("__Internal")]
		private static extern void _Send(string mailTo, string subject, string body);

		[DllImport("__Internal")]
		private static extern void _SendWithImage(string mailTo, string subject, string body, string imagePath);
#endif

		public static void Send(string mailTo, string subject, string body)
		{
#if UNITY_IOS && !UNITY_EDITOR
			_Send(mailTo, subject, body);
#else
			Application.OpenURL(string.Format("mailto:{0}?subject={1}&body={2}", mailTo, Uri.EscapeDataString(subject), Uri.EscapeDataString(body)));
#endif
		}

		public static void SendWithImage(string mailTo, string subject, string body, string imagePath)
		{
#if UNITY_IOS && !UNITY_EDITOR
			_SendWithImage(mailTo, subject, body, imagePath);
#else
			Debug.LogWarning("Sorry, cannot send an image from pc/osx.");
			Application.OpenURL(string.Format("mailto:{0}?subject={1}&body={2}&attachment={3}", mailTo, Uri.EscapeDataString(subject), Uri.EscapeDataString(body), Uri.EscapeDataString(imagePath)));
#endif
		}
	}
}