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

        private double Moment2Est;
        private double Moment3Est;
        private double Moment4Est;

        public NormalStatistics()
        {            
            KurtosisEst = Double.NaN;
            SkewnessEst = Double.NaN;            
        }

        public void Update(double observation)
        {
            NumberOfObservations++;
            var n = NumberOfObservations;
            var aux = (double)(n - 1) / (double)n;
            MeanEst = aux * MeanEst + observation / (double)n;            
            Moment2Est = aux * Moment2Est + Math.Pow(observation, 2) / (double)n;
            Moment3Est = aux * Moment3Est + Math.Pow(observation, 3) / (double)n;
            Moment4Est = aux * Moment4Est + Math.Pow(observation, 4) / (double)n;
        }

        public void Complete()
        {
            var n = NumberOfObservations;
            if (n > 1)
            {
                StdEst = Math.Sqrt((double)n / (double)(n - 1) * (Moment2Est - Math.Pow(MeanEst, 2)));

                double aux = Moment4Est - 4 * MeanEst * Moment3Est + 6 * Math.Pow(MeanEst, 2) * Moment2Est -
                    4 * Math.Pow(MeanEst, 4) + Math.Pow(MeanEst, 4) / (double)n;
                KurtosisEst = aux / Math.Pow(Math.Pow(StdEst, 2) * (double)(n - 1) / (double)n, 2);
                
                aux = Moment3Est - 3 * Moment2Est * MeanEst + 3 * Math.Pow(MeanEst, 3) - Math.Pow(MeanEst, 3) / (double)n;
                SkewnessEst = aux / Math.Pow(Math.Pow(StdEst, 2) * (double)(n - 1) / (double)n, 1.5);                
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
