using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Diagnostics.Logging;
using BlackjackSim.Simulation;

namespace BlackjackSim.Results
{
    public class AggregStatistics
    {
        public int AggregatedHandsCount { get; private set; }
        public int NumberOfObservations { get; private set; }
        public int NumberOfAggregatedObservations { get; private set; }

        public double MeanPal { get; private set; }
        public double StdPal{ get; private set; }
        public double MeanPalError { get; private set; }
        public double StdPalError { get; private set; }
        
        private double AggregatedPal;
        private double SumQuadAggregatedPal;
        private double TotalPal;

        public AggregStatistics(int aggregatedHandsCount)
        {
            AggregatedHandsCount = aggregatedHandsCount;
        }

        public void Update(BetHandResult betHandResult)
        {
            NumberOfObservations++;
            AggregatedPal += betHandResult.Payoff;
            if (NumberOfObservations % AggregatedHandsCount == 0)
            {
                NumberOfAggregatedObservations++;                
                SumQuadAggregatedPal += Math.Pow(AggregatedPal, 2);
                TotalPal += AggregatedPal;
                
                AggregatedPal = 0;
            }
        }

        public void ComputeEstimateErrors()
        {
            MeanPalError = 1.96 / Math.Sqrt((double)NumberOfAggregatedObservations) * StdPal;
            StdPalError = Math.Sqrt(1.96 * Math.Sqrt(2.0 / (double)NumberOfAggregatedObservations) *
                Math.Pow(StdPal, 2));
        }

        public void Complete()
        {
            MeanPal = TotalPal / (double)NumberOfAggregatedObservations;

            if (NumberOfAggregatedObservations > 1)
            {
                StdPal = Math.Sqrt((double)NumberOfAggregatedObservations / (double)(NumberOfAggregatedObservations - 1) *
                    (1.0 / (double)NumberOfAggregatedObservations * SumQuadAggregatedPal - Math.Pow(MeanPal, 2)));
            }

            ComputeEstimateErrors();
        }

        public void WriteToFile(StreamWriter writer)
        {
            Complete();

            try
            {
                string line;
                line = String.Format("*** {0} AGGREGATED HANDS RESULTS ***", AggregatedHandsCount);
                writer.WriteLine(line);
                line = String.Format("Mean PaL = {0} with Error {1}", MeanPal, MeanPalError);
                writer.WriteLine(line);
                line = String.Format("Std PaL = {0} with Error {1}", StdPal, StdPalError);
                writer.WriteLine(line);                
                line = String.Format("Nbr of Aggregated Hands Observations = {0}", NumberOfAggregatedObservations);
                writer.WriteLine(line);
            }
            catch (Exception ex)
            {
                TraceWrapper.LogException(ex, "Cannot write aggregated stats to a file!");
            }
        }
    }
}
