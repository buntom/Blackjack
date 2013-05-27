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
        public BetStats TotalStats;
        public List<TrueCountBetStatsBit> TrueCountStats;

        public Statistics()
        {
            TotalStats = new BetStats();
            TrueCountStats = new List<TrueCountBetStatsBit>();
        }

        public void Update(BetHandResult betHandResult)
        {
            TotalStats.Update(betHandResult);

            var trueCount = betHandResult.TrueCountBeforeBet;
            var trueCountBetStatsBit = TrueCountStats.Where(item => item.TrueCount == trueCount).FirstOrDefault();
            if (trueCountBetStatsBit != null)
            {
                trueCountBetStatsBit.BetStats.Update(betHandResult);
            }
            else
            {
                var newTrueCountBetStatsBit = new TrueCountBetStatsBit
                {
                    TrueCount = trueCount,
                    BetStats = new BetStats()
                };
                newTrueCountBetStatsBit.BetStats.Update(betHandResult);
                TrueCountStats.Add(newTrueCountBetStatsBit);
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
                        trueCountBetStatsBit.BetStats.WriteToFile(writer);
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
