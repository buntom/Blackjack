using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Configurations;
using BlackjackSim.Serialization;
using System.Threading;
using System.Globalization;
using Diagnostics.Logging;
using BlackjackSim.Simulation;
using BlackjackSim.Results;

namespace BlackjackSimRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (ArgumentsValid(args))
                {
                    var runner = new BlackjackSim.Runner(configurationPath: args[0]);
                    runner.Run();
                }
                else
                {
                    IncorrectArgumentsInfo(args);
                }
            }
            catch (Exception exception)
            {
                var message = "An exception occured in BlackjackSimRunner.";
                TraceWrapper.LogException(exception, message);
            }
        }

        static bool ArgumentsValid(string[] args)
        {
            return args.Length == 1 && System.IO.File.Exists(args[0]);
        }

        static void IncorrectArgumentsInfo(string[] args)
        {
            TraceWrapper.LogError("Incorrect input arguments: {0}", string.Join(" ", args));
            TraceWrapper.LogInformation("\tSample usage: BlackjackSimRunner \"configurationFile.xml\"");
        }
    }
}
