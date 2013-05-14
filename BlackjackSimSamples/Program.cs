using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSimSamples.Configurations;
using Diagnostics.Logging;
using BlackjackSimSamples.Diagnostics;
using BlackjackSimSamples.Simulation;
using BlackjackSimSamples.Strategies.Basic;

namespace BlackjackSimSamples
{
    class Program
    {
        static void Main(string[] args)
        {           
            try
            {
                //CardShoeSample.CreateShoe();
                //ConfigurationSample.CreateAndSaveSampleConfiguration();
                //DiagnosticsSample.TraceWrapperDemo();

                PairDecisionTableConfigurationTest.LoadConfiguration();
            }
            catch (Exception exception)
            {
                var message = "An exception occured in BlackjackSimSamples.";
                TraceWrapper.LogException(exception, message);
            }
        }
    }
}
