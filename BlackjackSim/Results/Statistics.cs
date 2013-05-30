using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Simulation;
using System.IO;
using Diagnostics.Logging;

namespace BlackjackSim.Results
{
    public class Statistics
    {
        public BetStatistics TotalBetStats;
        public AggregatedStatistics TotalAggregatedStats;
        public List<TrueCountBetStatsBit> TrueCountStats;

        private int AggregatedHandsCount;

        public Statistics(int aggregatedHandsCount)
        {
            TotalBetStats = new BetStatistics();
            TotalAggregatedStats = new AggregatedStatistics(aggregatedHandsCount);
            TrueCountStats = new List<TrueCountBetStatsBit>();

            AggregatedHandsCount = aggregatedHandsCount;
        }

        public void Update(BetHandResult betHandResult, StreamWriter aggregatedDataWriter)
        {
            TotalBetStats.Update(betHandResult);
            TotalAggregatedStats.UpdateAndLogData(betHandResult, aggregatedDataWriter);

            var trueCount = betHandResult.TrueCountBeforeBet;
            var trueCountBetStatsBit = TrueCountStats.Where(item => item.TrueCount == trueCount).FirstOrDefault();
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
                TrueCountStats.Add(newTrueCountBetStatsBit);
            }
        }

        public void WriteTrueCountStatisticsToFile(StreamWriter writer)
        {
            try
            {
                var header = "TrueCount, IBA, TBA, IBA Std, OptimalProportion";
                var delimiter = ",";
                writer.WriteLine(header);

                TrueCountStats = TrueCountStats.OrderBy(item => item.TrueCount).ToList();
                foreach (var trueCountBetStatsBit in TrueCountStats)
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
                TotalAggregatedStats.WriteToFile(writer);
                writer.WriteLine("");
                writer.WriteLine("*** SUMMARY: BET RESULTS ***");
                TotalBetStats.WriteToFile(writer);                
                if (TrueCountStats.Count > 0)
                {
                    TrueCountStats = TrueCountStats.OrderBy(item => item.TrueCount).ToList();
                    writer.WriteLine("");
                    writer.WriteLine("*** TRUE COUNT CONDITIONED RESULTS ***");
                    foreach (var trueCountBetStatsBit in TrueCountStats)
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
