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
        public StreamWriter AggregatedPalWriter;
        public Statistics Statistics;
        public AggregStatistics AggregStatistics;
        public string OutputFolder;

        private double AggregatedPal;
        public double Wealth { get; private set; }

        public ResultsUtils(Configuration configuration)
        {
            var simulationParameters = configuration.SimulationParameters;
            OutputFolder = simulationParameters.OutputFolderSpecific;
            Directory.CreateDirectory(OutputFolder);

            if (simulationParameters.SaveFullResults)
            {
                ResultsWriter = InitiateResultsLog(OutputFolder, "BlackjackSim_Results.csv");
                ResultsWriter.WriteLine(BetHandResult.GetHeader());
            }
            
            var aggregatedHandsCount = simulationParameters.AggregStatsHandCount;
            if (simulationParameters.SaveAggregatedPal)
            {
                var logName = "BlackjackSim_" + String.Format("{0}", aggregatedHandsCount) +
                    "playedHandsPal.csv";
                AggregatedPalWriter = InitiateResultsLog(OutputFolder, logName);
            }
            
            AggregStatistics = new AggregStatistics(aggregatedHandsCount);
            Statistics = new Statistics();
            Wealth = simulationParameters.InitialWealth;
        }

        public void Update(BetHandResult betHandResult, int iteration)
        {            
            Statistics.Update(betHandResult);
            AggregStatistics.Update(betHandResult);
            DumpToResultsLog(betHandResult);
            LogAggregatedPal(betHandResult.Payoff, iteration);
            UpdateWealth(betHandResult.Payoff);
        }

        public void UpdateWealth(double payoff)
        {
            Wealth += payoff;
        }

        public void TrueCountStatisticsToFile()
        {
            try
            {
                var filePath = Path.Combine(OutputFolder, "BlackjackSim_TrueCountStatistics.csv");
                var writer = new StreamWriter(filePath);
                Statistics.WriteTrueCountStatisticsToFile(writer);
                writer.Close();
            }
            catch (Exception ex)
            {
                TraceWrapper.LogException(ex, "Cannot save true count statistics to a file!");
            }
        }

        public void SummaryStatisticsToFile()
        {
            try
            {                
                var filePath = Path.Combine(OutputFolder, "BlackjackSim_Summary.txt");
                var writer = new StreamWriter(filePath);
                AggregStatistics.WriteToFile(writer);
                writer.WriteLine("");
                Statistics.WriteToFile(writer);
                writer.Close();
            }
            catch (Exception ex)
            {
                TraceWrapper.LogException(ex, "Cannot save summary statistics to a file!");
            }
        }

        public static StreamWriter InitiateResultsLog(string folder, string fileName)
        {
            try
            {                
                var filePath = Path.Combine(folder, fileName);
                var writer = new StreamWriter(filePath);                

                return writer;
            }
            catch (Exception ex)
            {                
                TraceWrapper.LogException(ex, "Cannot initiate results log file: " + fileName);
                return null;
            }
        }

        public void LogAggregatedPal(double payoff, int iteration)
        {
            if (AggregatedPalWriter != null)
            {
                AggregatedPal += payoff;
                var aggregatedHandsCount = AggregStatistics.AggregatedHandsCount;
                if (iteration % aggregatedHandsCount == 0)
                {
                    if (AggregatedPalWriter != null)
                    {
                        try
                        {
                            AggregatedPalWriter.WriteLine(String.Format("{0}", AggregatedPal));
                        }
                        catch (Exception ex)
                        {
                            TraceWrapper.LogException(ex, "Cannot save aggregated PaL to a txt file!");
                        }
                    }
                    AggregatedPal = 0;
                }
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

        public void CloseAggregatedPalLog()
        {
            if (AggregatedPalWriter != null)
            {
                AggregatedPalWriter.Close();
            }
        }

        public void CloseResultsLog()
        {
            if (ResultsWriter != null)
            {
                ResultsWriter.Close();
            }
        }

        public void FinalizeAll()
        {
            CloseAggregatedPalLog();
            CloseResultsLog();
            SummaryStatisticsToFile();
            TrueCountStatisticsToFile();
        }
    }
}
