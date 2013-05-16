using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Strategies;

namespace BlackjackSim.Configurations.Strategies.Basic
{
    public static class DecisionTypeHelper
    {
        public static DecisionType ConvertFromString(string codeString)
        {
            switch (codeString.Trim().ToUpper())
            {
                case "0":
                    return DecisionType.STAND;

                case "1":
                    return DecisionType.HIT;

                case "2":
                    return DecisionType.DOUBLE_OR_HIT;

                case "3":
                    return DecisionType.DOUBLE_OR_STAND;

                case "4":
                    return DecisionType.SURRENDER_OR_HIT;

                case "5":
                    return DecisionType.SURRENDER_OR_STAND;

                default:
                    throw new ArgumentException(string.Format("Unknown decision code: {0}", codeString));
            }
        }

        public static StrategyDecisionType ConvertToStrategyDecision(DecisionType decision, Permits permits)
        {
            if (decision == DecisionType.STAND)
            {
                return StrategyDecisionType.STAND;
            }
            if (decision == DecisionType.HIT)
            {
                return StrategyDecisionType.HIT;
            }
            if (decision == DecisionType.DOUBLE_OR_HIT && !permits.Double)
            {
                return StrategyDecisionType.HIT;
            }
            if (decision == DecisionType.DOUBLE_OR_HIT && permits.Double)
            {
                return StrategyDecisionType.DOUBLE;
            }
            if (decision == DecisionType.DOUBLE_OR_STAND && !permits.Double)
            {
                return StrategyDecisionType.STAND;
            }
            if (decision == DecisionType.DOUBLE_OR_STAND && permits.Double)
            {
                return StrategyDecisionType.DOUBLE;
            }
            if (decision == DecisionType.SURRENDER_OR_HIT && !permits.Surrender)
            {
                return StrategyDecisionType.HIT;
            }
            if (decision == DecisionType.SURRENDER_OR_HIT && permits.Surrender)
            {
                return StrategyDecisionType.SURRENDER;
            }
            if (decision == DecisionType.SURRENDER_OR_STAND && !permits.Surrender)
            {
                return StrategyDecisionType.STAND;
            }
            if (decision == DecisionType.DOUBLE_OR_STAND && permits.Surrender)
            {
                return StrategyDecisionType.SURRENDER;
            }

            return StrategyDecisionType.NA;
        }
    }
}