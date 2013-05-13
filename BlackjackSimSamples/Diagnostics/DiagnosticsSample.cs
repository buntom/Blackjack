using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Diagnostics.Logging;

namespace BlackjackSimSamples.Diagnostics
{
    public static class DiagnosticsSample
    {
        public static void TraceWrapperDemo()
        {
            TraceWrapper.LogInformation("Information");
            TraceWrapper.LogVerbose("Verbose");
            TraceWrapper.LogWarning("Warning");
            TraceWrapper.LogError("Error");
        }
    }
}
