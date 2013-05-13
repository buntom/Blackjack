using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Diagnostics.Logging
{
    public static class TraceWrapper
    {
        public static LogMessageHandler OnLogError { get; set; }
        public static LogMessageHandler OnLogInformation { get; set; }
        public static LogMessageHandler OnLogInformationWithoutNewline { get; set; }
        public static LogMessageHandler OnLogVerbose { get; set; }
        public static LogMessageHandler OnLogWarning { get; set; }

        public static void LogError(string format, params object[] args)
        {
            var message = string.Format(format, args);

            ConsoleUtils.ConsoleErrorColor();

            if (TraceWrapper.OnLogError != null)
            {
                TraceWrapper.OnLogError(message);
            }
            else
            {
                DefaultLogger(message);
            }

            ConsoleUtils.ConsoleResetColor();
        }

        public static void LogInformation(string format, params object[] args)
        {
            var message = string.Format(format, args);

            ConsoleUtils.ConsoleInformationColor();

            if (TraceWrapper.OnLogInformation != null)
            {
                TraceWrapper.OnLogInformation(message);
            }
            else
            {
                DefaultLogger(message);
            }

            ConsoleUtils.ConsoleResetColor();
        }

        public static void LogInformationWithoutNewline(string format, params object[] args)
        {
            var message = string.Format(format, args);

            ConsoleUtils.ConsoleInformationColor();

            if (TraceWrapper.OnLogInformationWithoutNewline != null)
            {
                TraceWrapper.OnLogInformationWithoutNewline(message);
            }
            else
            {
                DefaultLogger(message, addNewLine: false);
            }

            ConsoleUtils.ConsoleResetColor();
        }

        public static void LogVerbose(string format, params object[] args)
        {
            var message = string.Format(format, args);

            ConsoleUtils.ConsoleVerboseColor();

            if (TraceWrapper.OnLogVerbose != null)
            {
                TraceWrapper.OnLogVerbose(message);
            }
            else
            {
                DefaultLogger(message);
            }

            ConsoleUtils.ConsoleResetColor();
        }

        public static void LogWarning(string format, params object[] args)
        {
            var message = string.Format(format, args);

            ConsoleUtils.ConsoleWarningColor();

            if (TraceWrapper.OnLogWarning != null)
            {
                TraceWrapper.OnLogWarning(message);
            }
            else
            {
                DefaultLogger(message);
            }

            ConsoleUtils.ConsoleResetColor();
        }

        public static void LogSeparatorLine()
        {
            LogInformation(new String('-', 60));
        }

        public static void LogException(Exception exception, string format = null, params object[] args)
        {
            if (!string.IsNullOrEmpty(format))
            {
                LogError("");
                LogError(format, args);
            }

            LogError("");
            LogError("Error message: {0}", exception.Message);
            LogWarning("");
            LogWarning("Source: {0}", exception.Source);
            LogWarning("");
            LogWarning("Stack trace: {0}", exception.StackTrace);
        }

        private static void DefaultLogger(string message, bool addNewLine = true)
        {
            if (addNewLine)
            {
                Trace.WriteLine(message);
            }
            else
            {
                Trace.Write(message);
            }
        }
    }
}