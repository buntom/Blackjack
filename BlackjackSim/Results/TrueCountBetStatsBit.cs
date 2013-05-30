using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Simulation;
using System.IO;

namespace BlackjackSim.Results
{
    public class TrueCountBetStatsBit
    {
        public int TrueCount { get; set; }
        public BetStatistics BetStatistics { get; set; }
        public AggregatedStatistics AggregatedStatistics { get; set; }

        public void Update(BetHandResult betHandResult)
        {
            BetStatistics.Update(betHandResult);
            AggregatedStatistics.UpdateAndLogData(betHandResult);
        }

        public void WriteToFile(StreamWriter writer)
        {
            var line = String.Format("<-- TRUE COUNT = {0}:", TrueCount);
            writer.WriteLine(line);
            BetStatistics.WriteToFile(writer);
            writer.WriteLine("---");
            AggregatedStatistics.WriteToFile(writer);
            writer.WriteLine("-->");
            writer.WriteLine("");
        }
    }
}
