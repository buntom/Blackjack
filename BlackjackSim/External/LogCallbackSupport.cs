using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Diagnostics.Logging;

namespace BlackjackSim.External
{
    public static class LogCallbackSupport
    {
        private static List<string> CallbackSetterIds;

        static LogCallbackSupport()
        {
            CallbackSetterIds = new List<string>();
        }

        public static void CallbackSet(string setterId)
        {
            if (!IsCallbackSet(setterId))
            {
                CallbackSetterIds.Add(setterId);
            }

            TraceWrapper.LogInformation("External print callbacks set, setter ID: {0}", setterId);
        }

        public static bool IsCallbackSet(string setterId)
        {
            return CallbackSetterIds.Contains(setterId);
        }

        public static void SetInformationCallback(Action<string> callback)
        {
            TraceWrapper.OnLogInformation += message => callback(message);
        }

        public static void SetInformationWithoutNewlineCallback(Action<string> callback)
        {
            TraceWrapper.OnLogInformationWithoutNewline += message => callback(message);
        }

        public static void SetVerboseCallback(Action<string> callback)
        {
            TraceWrapper.OnLogVerbose += message => callback(message);
        }

        public static void SetWarningCallback(Action<string> callback)
        {
            TraceWrapper.OnLogWarning += message => callback(message);
        }

        public static void SetErrorCallback(Action<string> callback)
        {
            TraceWrapper.OnLogError += message => callback(message);
        }
    }
}
