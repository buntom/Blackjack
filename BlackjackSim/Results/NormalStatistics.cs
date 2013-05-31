using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Diagnostics.Logging;

namespace BlackjackSim.Results
{
    public class NormalStatistics
    {
        public double MeanEst { get; private set; }
        public double StdEst { get; private set; }
        public double MeanEstError { get; private set; }
        public double StdEstError { get; private set; }
        public double KurtosisEst { get; private set; }
        public double SkewnessEst { get; private set; }
        public int NumberOfObservations { get; private set; }

        private double Sum2Moment;
        private double Sum3Moment;
        private double Sum4Moment;

        public NormalStatistics()
        {            
            KurtosisEst = Double.NaN;
            SkewnessEst = Double.NaN;            
        }

        public void Update(double observation)
        {
            NumberOfObservations++;
            MeanEst = (MeanEst * (NumberOfObservations - 1) + observation) / (double)NumberOfObservations;
            Sum2Moment += Math.Pow(observation, 2);
            Sum3Moment += Math.Pow(observation, 3);
            Sum4Moment += Math.Pow(observation, 4);
        }

        public void Complete()
        {
            var n = NumberOfObservations;
            if (n > 1)
            {
                StdEst = Math.Sqrt((double)n / (double)(n - 1) * (1.0 / (double)n * Sum2Moment - Math.Pow(MeanEst, 2)));

                double aux = Sum4Moment - 4 * MeanEst * Sum3Moment + 6 * Math.Pow(MeanEst, 2) * Sum2Moment -
                    4 * Math.Pow(MeanEst, 3) * MeanEst * n + Math.Pow(MeanEst, 4);
                KurtosisEst = (aux / (double)n) / Math.Pow(Math.Pow(StdEst, 2) * (double)(n - 1) / (double)n, 2);

                aux = Sum3Moment - 3 * Sum2Moment * MeanEst + 3 * MeanEst * n * Math.Pow(MeanEst, 2) - Math.Pow(MeanEst, 3);
                SkewnessEst = (aux / (double)n) / Math.Pow(Math.Pow(StdEst, 2) * (double)(n - 1) / (double)n, 1.5);
            }

            MeanEstError = 1.96 / Math.Sqrt((double)n) * StdEst;
            StdEstError = Math.Sqrt(1.96 * Math.Sqrt(2.0 / (double)n) * Math.Pow(StdEst, 2));            
        }

        public void WriteToFile(StreamWriter writer)
        {
            Complete();

            try
            {
                string line;
                line = String.Format("MeanEst = {0} with Error {1}", MeanEst, MeanEstError);
                writer.WriteLine(line);
                line = String.Format("StdEst = {0} with Error {1}", StdEst, StdEstError);
                writer.WriteLine(line);
                line = String.Format("KurtosisEst = {0}, SkewnessEst = {1}", KurtosisEst, SkewnessEst);
                writer.WriteLine(line);
                line = String.Format("NbrOfObservations = {0}", NumberOfObservations);
                writer.WriteLine(line);
            }
            catch (Exception ex)
            {
                TraceWrapper.LogException(ex, "Cannot write normal stats to a file!");
            }
        }
    }
}
