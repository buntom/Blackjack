using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Configurations;

namespace BlackjackSim.Simulation
{
    public class Count
    {
        public double RunningCount { get; private set; }
        public int TrueCount { get; private set; }

        private readonly List<CountSystemBit> CountSystem;

        public Count(Configuration configuration)
        {
            CountSystem = configuration.SimulationParameters.CountSystem;
        }

        public void Update(int card, double leftPacksCount)
        {

            var countSystemBit = CountSystem.Where(bit => bit.Card == card).FirstOrDefault();

            bool bitFound = countSystemBit != null;
            if (bitFound)
            {
                RunningCount += countSystemBit.Count;
            }
            
            TrueCount = (int)Math.Truncate(RunningCount / leftPacksCount);
        }

        public void Update(List<int> cards, double leftPacksCount)
        {
            foreach (var card in cards)
            {
                var countSystemBit = CountSystem.Where(bit => bit.Card == card).FirstOrDefault();

                bool bitFound = countSystemBit != null;
                if (bitFound)
                {
                    RunningCount += countSystemBit.Count;
                }
            }

            TrueCount = (int)Math.Truncate(RunningCount / leftPacksCount);
        }

        public void Reset()
        {
            RunningCount = 0;
            TrueCount = 0;
        }

    }
}
