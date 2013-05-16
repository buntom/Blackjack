using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Simulation;

namespace BlackjackSim.Strategies
{
    public interface IStrategy
    {
        bool GetInsuranceDecision(Hand handPlayer, int trueCount);        
        StrategyDecisionType GetDecision(Hand handPlayer, Hand handDealer, int trueCount, Permits permits, Random random);
    }
}
