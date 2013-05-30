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
        public int NumberOfObservations { get; private set; }

        private double SumQuad;

        public void Update(double observation)
        {
            NumberOfObservations++;
            MeanEst = (MeanEst * (NumberOfObservations - 1) + observation) / (double)NumberOfObservations;
            SumQuad += Math.Pow(observation, 2);
        }

        public void Complete()
        {
            if (NumberOfObservations > 1)
            {
                StdEst = Math.Sqrt((double)NumberOfObservations / (double)(NumberOfObservations - 1) *
                        (1.0 / (double)NumberOfObservations * SumQuad - Math.Pow(MeanEst, 2)));
            }
            MeanEstError = 1.96 / Math.Sqrt((double)NumberOfObservations) * StdEst;
            StdEstError = Math.Sqrt(1.96 * Math.Sqrt(2.0 / (double)NumberOfObservations) *
                Math.Pow(StdEst, 2));
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
