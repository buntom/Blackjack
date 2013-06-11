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
    public class Statistics
    {
        public BetStatistics TotalBetStatistics;
        public AggregatedStatistics TotalAggregatedStatistics;
        public WealthStatistics WealthStatistics;
        public List<TrueCountBetStatsBit> TrueCountStatistics;

        private int AggregatedHandsCount;

        public Statistics(Configuration configuration)
        {
            AggregatedHandsCount = configuration.SimulationParameters.AggregStatsHandCount;

            TotalBetStatistics = new BetStatistics();
            TotalAggregatedStatistics = new AggregatedStatistics(AggregatedHandsCount);
            WealthStatistics = new WealthStatistics(configuration);
            TrueCountStatistics = new List<TrueCountBetStatsBit>();            
        }

        public void Update(BetHandResult betHandResult, StreamWriter aggregatedDataWriter)
        {
            TotalBetStatistics.Update(betHandResult);
            TotalAggregatedStatistics.UpdateAndLogData(betHandResult, aggregatedDataWriter);
            WealthStatistics.Update(betHandResult);

            var trueCount = betHandResult.TrueCountBeforeBet;
            var trueCountBetStatsBit = TrueCountStatistics.Where(item => item.TrueCount == trueCount).FirstOrDefault();
            if (trueCountBetStatsBit != null)
            {
                trueCountBetStatsBit.Update(betHandResult);                
            }
            else
            {
                var newTrueCountBetStatsBit = new TrueCountBetStatsBit
                {
                    TrueCount = trueCount,
                    BetStatistics = new BetStatistics(),
                    AggregatedStatistics = new AggregatedStatistics(AggregatedHandsCount)
                };
                newTrueCountBetStatsBit.Update(betHandResult);                
                TrueCountStatistics.Add(newTrueCountBetStatsBit);
            }
        }

        public void WriteTrueCountStatisticsToFile(StreamWriter writer)
        {
            try
            {
                var header = "TrueCount, IBA, TBA, IBA Std, OptimalProportion";
                var delimiter = ",";
                writer.WriteLine(header);

                TrueCountStatistics = TrueCountStatistics.OrderBy(item => item.TrueCount).ToList();
                foreach (var trueCountBetStatsBit in TrueCountStatistics)
                {
                    var betStats = trueCountBetStatsBit.BetStatistics;
                    var line = String.Format("{0}" + delimiter + " {1}" + delimiter + 
                        " {2}" + delimiter + " {3}" + delimiter + " {4}",
                        trueCountBetStatsBit.TrueCount, betStats.InitialBetAdvantage, 
                        betStats.TotalBetAdvantage, betStats.StdIba,
                        betStats.InitialBetAdvantage / Math.Pow(betStats.StdIba, 2));
                    writer.WriteLine(line);                        
                }
            }
            catch (Exception ex)
            {
                TraceWrapper.LogException(ex, "Cannot write true count statistics to a file!");
            }
        }

        public void WriteToFile(StreamWriter writer)
        {
            try
            {
                writer.WriteLine("*** SUMMARY: AGGREGATED HANDS RESULTS ***");
                TotalAggregatedStatistics.WriteToFile(writer);
                writer.WriteLine("");

                writer.WriteLine("*** SUMMARY: BET RESULTS ***");
                TotalBetStatistics.WriteToFile(writer);                
                if (TrueCountStatistics.Count > 0)
                {
                    TrueCountStatistics = TrueCountStatistics.OrderBy(item => item.TrueCount).ToList();
                    writer.WriteLine("");
                    writer.WriteLine("*** TRUE COUNT CONDITIONED RESULTS ***");
                    foreach (var trueCountBetStatsBit in TrueCountStatistics)
                    {
                        trueCountBetStatsBit.WriteToFile(writer);
                    }                    
                }
            }
            catch (Exception ex)
            {
                TraceWrapper.LogException(ex, "Cannot write statistics to a file!");
            }
        }
    }
}
