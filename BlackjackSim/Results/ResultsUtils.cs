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

        public double InitialWealth { get; private set; }
        public double Wealth { get; private set; }
        public double MinWealth { get; private set; }
        public double MaxWealth { get; private set; }

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
            InitialWealth = Wealth;
            MinWealth = Wealth;
            MaxWealth = Wealth;

            if (simulationParameters.SaveAggregatedData)
            {                
                AggregatedDataWriter = InitiateResultsLog(OutputFolder, "BlackjackSim_AggregatedHandsData.csv");
                Statistics.TotalAggregatedStatistics.LogHeader(AggregatedDataWriter);
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
            MinWealth = Math.Min(MinWealth, Wealth);
            MaxWealth = Math.Max(MaxWealth, Wealth);
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

        public void WealthStatisticsToFile(StreamWriter writer)
        {
            try
            {
                writer.WriteLine("*** WEALTH RESULTS ***");
                string line;
                if (Wealth <= 0)
                {
                    line = String.Format("Bankruptcy has occured after {0} played hands, simulation was terminated prematurely!",
                        Statistics.TotalAggregatedStatistics.NumberOfObservations);
                    writer.WriteLine(line);
                }
                line = String.Format("Initial Wealth = {0}", InitialWealth);
                writer.WriteLine(line);
                line = String.Format("Final Wealth = {0}", Wealth);
                writer.WriteLine(line);
                line = String.Format("Min Wealth = {0}", MinWealth);
                writer.WriteLine(line);
                line = String.Format("Max Wealth = {0}", MaxWealth);
                writer.WriteLine(line);
                writer.WriteLine("");
            }
            catch (Exception ex)
            {
                TraceWrapper.LogException(ex, "Cannot save wealth statistics to a file!");
            }
        }

        public void SummaryStatisticsToFile()
        {
            try
            {                
                var filePath = Path.Combine(OutputFolder, "BlackjackSim_Summary.txt");
                var writer = new StreamWriter(filePath);
                WealthStatisticsToFile(writer);
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
