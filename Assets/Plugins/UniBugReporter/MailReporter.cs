using UniMail;
using UnityEngine;

namespace UniBugReporter
{
    public class MailReporter : IReporter
    {
        private const string MailTo = "dummy@example.com";
        private const string Subject = "BugReport";

        public bool IsReportTarget(string message, string stackTrace, LogType type)
        {
            return type == LogType.Assert || type == LogType.Exception;
        }

        public void Report(string message, string stackTrace, LogType type, string screenshotFilePath)
        {
            var body = "{ERROR_MESSAGE}\n---\n{STACK_TRACE}";
            body = body.Replace("{ERROR_MESSAGE}", message);
            body = body.Replace("{STACK_TRACE}", stackTrace);

            if (string.IsNullOrEmpty(screenshotFilePath))
            {
                Mail.Send(MailTo, Subject, body);
            }
            else
            {
                Mail.SendWithImage(MailTo, Subject, body, screenshotFilePath);
            }
        }
    }
}
