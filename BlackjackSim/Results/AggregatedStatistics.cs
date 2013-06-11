using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Diagnostics.Logging;
using BlackjackSim.Simulation;
using BlackjackSim.Configurations;

namespace BlackjackSim.Results
{
    public class AggregatedStatistics
    {
        public int AggregatedHandsCount { get; private set; }
        public long NumberOfObservations { get; private set; }
        public int NumberOfAggregatedObservations { get; private set; }

        public NormalStatistics PalStatistics;
        public NormalStatistics IbaStatistics;
        public NormalStatistics TbaStatistics;
        
        public double AggregatedPal { get; private set; }
        public double AggregatedTotalBet { get; private set; }
        public double AggregatedInitialBet { get; private set; }
                        
        public AggregatedStatistics(int aggregatedHandsCount)
        {
            AggregatedHandsCount = aggregatedHandsCount;
            PalStatistics = new NormalStatistics();
            IbaStatistics = new NormalStatistics();
            TbaStatistics = new NormalStatistics();
        }

        public void UpdateAndLogData(BetHandResult betHandResult, StreamWriter writer = null)
        {
            NumberOfObservations++;
            AggregatedPal += betHandResult.Payoff;
            AggregatedTotalBet += betHandResult.BetTotal;
            AggregatedInitialBet += betHandResult.BetSize;
            if (NumberOfObservations % AggregatedHandsCount == 0)
            {
                FlushResetAndLogData(writer);
            }
        }

        public void FlushResetAndLogData(StreamWriter writer)
        {
            NumberOfAggregatedObservations++;
            var AggregatedIba = AggregatedPal / AggregatedInitialBet;
            var AggregatedTba = AggregatedPal / AggregatedTotalBet;

            PalStatistics.Update(AggregatedPal);
            IbaStatistics.Update(AggregatedIba);
            TbaStatistics.Update(AggregatedTba);

            LogData(writer);

            AggregatedPal = 0;
            AggregatedTotalBet = 0;
            AggregatedInitialBet = 0;
        }

        public void LogHeader(StreamWriter writer)
        {
            writer.WriteLine("PaL, IBA, TBA");
        }

        public void LogData(StreamWriter writer)
        {
            if (writer != null)
            {
                try
                {
                    var AggregatedIba = AggregatedPal / AggregatedInitialBet;
                    var AggregatedTba = AggregatedPal / AggregatedTotalBet;
                    writer.WriteLine(String.Format("{0}, {1}, {2}",
                        AggregatedPal, AggregatedIba, AggregatedTba));
                }
                catch (Exception ex)
                {
                    TraceWrapper.LogException(ex, "Cannot log aggregated data to a txt file!");
                }
            }
        }
        
        public void WriteToFile(StreamWriter writer)
        {
            try
            {                
                var line = String.Format("{0} aggregated hands results:", AggregatedHandsCount);
                writer.WriteLine(line);
                writer.WriteLine("PaL Statistics:");
                PalStatistics.WriteToFile(writer);
                writer.WriteLine("---");
                writer.WriteLine("IBA Statistics:");
                IbaStatistics.WriteToFile(writer);
                writer.WriteLine("---");
                writer.WriteLine("TBA Statistics:");
                TbaStatistics.WriteToFile(writer);
            }
            catch (Exception ex)
            {
                TraceWrapper.LogException(ex, "Cannot write aggregated stats to a file!");
            }
        }
    }
}
