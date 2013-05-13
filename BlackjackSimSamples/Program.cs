using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSimSamples.Configurations;
using Diagnostics.Logging;
using BlackjackSimSamples.Diagnostics;
using BlackjackSimSamples.Simulation;

namespace BlackjackSimSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            CardShoeSample.CreateShoe();

            //try
            //{
            //    ConfigurationSample.CreateAndSaveSampleConfiguration();
            //    DiagnosticsSample.TraceWrapperDemo();
            //}
            //catch (Exception exception)
            //{
            //    var message = "An exception occured in BlackjackSimSamples.";
            //    TraceWrapper.LogException(exception, message);
            //}
        }
    }
}
