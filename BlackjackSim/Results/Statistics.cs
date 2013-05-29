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
        public BetStatistics TotalStats;        
        public List<TrueCountBetStatsBit> TrueCountStats;

        public Statistics()
        {
            TotalStats = new BetStatistics();            
            TrueCountStats = new List<TrueCountBetStatsBit>();
        }

        public void Update(BetHandResult betHandResult)
        {
            TotalStats.Update(betHandResult);

            var trueCount = betHandResult.TrueCountBeforeBet;
            var trueCountBetStatsBit = TrueCountStats.Where(item => item.TrueCount == trueCount).FirstOrDefault();
            if (trueCountBetStatsBit != null)
            {
                trueCountBetStatsBit.BetStatistics.Update(betHandResult);                
            }
            else
            {
                var newTrueCountBetStatsBit = new TrueCountBetStatsBit
                {
                    TrueCount = trueCount,
                    BetStatistics = new BetStatistics()                    
                };
                newTrueCountBetStatsBit.BetStatistics.Update(betHandResult);
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
                writer.WriteLine("*** SUMMARY RESULTS ***");
                TotalStats.WriteToFile(writer);                
                if (TrueCountStats.Count > 0)
                {
                    TrueCountStats = TrueCountStats.OrderBy(item => item.TrueCount).ToList();
                    writer.WriteLine("");
                    writer.WriteLine("*** TRUE COUNT CONDITIONED RESULTS ***");
                    foreach (var trueCountBetStatsBit in TrueCountStats)
                    {
                        var line = String.Format("TRUE COUNT = {0}:", trueCountBetStatsBit.TrueCount);
                        writer.WriteLine(line);                        
                        trueCountBetStatsBit.BetStatistics.WriteToFile(writer);
                        writer.WriteLine("");
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
