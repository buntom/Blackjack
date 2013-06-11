using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Configurations;
using BlackjackSim.Simulation;
using System.IO;
using Diagnostics.Logging;

namespace BlackjackSim.Results
{
    // TODO: add some wealth and cosumption related stats; consider situation after bankruptcy...
    public class WealthStatistics
    {        
        public double WealthValue { get; private set; }
        public double ConsumptionValue { get; private set; }

        private double InitialWealth;
        private double ConsumptionRate;
        private int AggregatedHandsCount;
        private int NumberOfObservations;

        public WealthStatistics(Configuration configuration)
        {
            InitialWealth = configuration.SimulationParameters.InitialWealth;
            ConsumptionRate = configuration.SimulationParameters.ConsumptionRate;
            AggregatedHandsCount = configuration.SimulationParameters.AggregStatsHandCount;
                 
            WealthValue = InitialWealth;
        }

        public void Update(BetHandResult betHandResult)
        {
            NumberOfObservations++;
            WealthValue += betHandResult.Payoff;
            if (ConsumptionRate > 0)
            {
                var toConsume = Math.Round(WealthValue * ConsumptionRate);
                ConsumptionValue += toConsume;
                WealthValue -= toConsume;                
            }

            if (NumberOfObservations % AggregatedHandsCount == 0)
            {
                Reset();
            }

            if (WealthValue <= 0)
            {
                var message = String.Format("Bankruptcy has occured in {0}th played hand, wealth reset to initial value!", NumberOfObservations);
                TraceWrapper.LogInformation(message);

                Reset();
            }            
        }

        public void Reset()
        {
            WealthValue = InitialWealth;
            ConsumptionValue = 0;
        }
    }
}
