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
        public StreamWriter AggregatedDataWriter;
        public Statistics Statistics;        
        public string OutputFolder;

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
            Statistics = new Statistics(aggregatedHandsCount);
            Wealth = simulationParameters.InitialWealth;

            if (simulationParameters.SaveAggregatedData)
            {                
                AggregatedDataWriter = InitiateResultsLog(OutputFolder, "BlackjackSim_AggregatedHandsData.csv");
                Statistics.TotalAggregatedStats.LogHeader(AggregatedDataWriter);
            }
        }

        public void Update(BetHandResult betHandResult)
        {            
            Statistics.Update(betHandResult, AggregatedDataWriter);            
            DumpToResultsLog(betHandResult);            
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

        public void CloseAggregatedDataLog()
        {
            if (AggregatedDataWriter != null)
            {
                AggregatedDataWriter.Close();
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
            CloseAggregatedDataLog();
            CloseResultsLog();
            SummaryStatisticsToFile();
            TrueCountStatisticsToFile();
        }
    }
}
