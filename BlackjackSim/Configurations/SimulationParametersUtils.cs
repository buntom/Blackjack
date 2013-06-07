using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackSim.Configurations
{
    public static class SimulationParametersUtils
    {
        public static TrueCountBet GetTrueCountBet(this List<TrueCountBet> betSizeTrueCountScale, int trueCount)
        {
            var minTrueCountBet = betSizeTrueCountScale.First();
            var maxTrueCountBet = betSizeTrueCountScale.Last();

            var trueCountBet = betSizeTrueCountScale.Where(item => item.TrueCount == trueCount).FirstOrDefault();
            if (trueCountBet != null)
            {
                return trueCountBet;
            }
            else if (trueCount < minTrueCountBet.TrueCount)
            {
                return minTrueCountBet;
            }
            else if (trueCount > maxTrueCountBet.TrueCount)
            {
                return maxTrueCountBet;
            }
            else
            {
                return null;
            }            
        }
    }
}
