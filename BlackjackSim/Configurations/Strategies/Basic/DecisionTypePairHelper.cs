using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Strategies;

namespace BlackjackSim.Configurations.Strategies.Basic
{
    public static class DecisionTypePairHelper
    {
        public static DecisionTypePair ConvertFromString(string codeString)
        {
            switch (codeString.Trim().ToUpper())
            {
                case "0":
                    return DecisionTypePair.SPLIT_NOT;

                case "1":
                    return DecisionTypePair.SPLIT;

                case "2":
                    return DecisionTypePair.SURRENDER_OR_SPLIT;

                default:
                    throw new ArgumentException(string.Format("Unknown pair decision code: {0}", codeString));
            }
        }

        public static StrategyDecisionType ConvertToStrategyDecision(DecisionTypePair pairDecision, Permits permits)
        {
            if (pairDecision == DecisionTypePair.SURRENDER_OR_SPLIT && permits.Surrender)
            {
                return StrategyDecisionType.SURRENDER;
            }
            else if (pairDecision == DecisionTypePair.SURRENDER_OR_SPLIT && !permits.Surrender)
            {
                pairDecision = DecisionTypePair.SPLIT;
            }
            if (pairDecision == DecisionTypePair.SPLIT && permits.Split)
            {
                return StrategyDecisionType.SPLIT;
            }

            return StrategyDecisionType.NA;
        }
    }
}
