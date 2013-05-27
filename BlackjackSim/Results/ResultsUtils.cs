using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Simulation;
using System.IO;
using Diagnostics.Logging;
using BlackjackSim.Configurations;

namespace BlackjackSim.Results
{
    public class ResultsUtils
    {
        public StreamWriter ResultsWriter;
        public Statistics Statistics;
        public string OutputFolder;

        public ResultsUtils(Configuration configuration)
        {
            var simulationParameters = configuration.SimulationParameters;
            OutputFolder = simulationParameters.OutputFolderSpecific;
            Directory.CreateDirectory(OutputFolder);
            if (simulationParameters.SaveResults)
            {                
                ResultsWriter = InitiateResultsLog(OutputFolder);
            }
            else
            {
                ResultsWriter = null;
            }

            Statistics = new Statistics();
        }

        public void StatisticsToFile()
        {
            try
            {                
                var filePath = Path.Combine(OutputFolder, "BJsim_Summary.txt");
                var writer = new StreamWriter(filePath);
                Statistics.WriteToFile(writer);
                writer.Close();
            }
            catch (Exception ex)
            {
                TraceWrapper.LogException(ex, "Cannot save statistics to a file!");
            }
        }

        public static StreamWriter InitiateResultsLog(string folder)
        {
            try
            {                
                var filePath = Path.Combine(folder, "BJsim_Results.csv");
                var writer = new StreamWriter(filePath);
                writer.WriteLine(BetHandResult.GetHeader());

                return writer;
            }
            catch (Exception ex)
            {                
                TraceWrapper.LogException(ex, "Cannot initiate results log file!");
                return null;
            }
        }

        public void DumpToResultsLog(BetHandResult betHandResult)
        {
            if (ResultsWriter != null)
            {
                try
                {
                    ResultsWriter.WriteLine(betHandResult.ConvertToString());
                }
                catch (Exception ex)
                {
                    TraceWrapper.LogException(ex, "Cannot save results to a txt file!");
                }
            }
        }

        public void CloseResultsLog()
        {
            if (ResultsWriter != null)
            {
                ResultsWriter.Close();
            }
        }
    }
}
