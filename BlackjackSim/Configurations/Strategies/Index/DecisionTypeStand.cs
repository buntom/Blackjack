using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackSim.Configurations.Strategies.Index
{
    public struct DecisionTypeStand
    {
        public DecisionTypeBase DecisionBase;

        public static DecisionTypeStand ConvertFromString(string codeString)
        {
            var decisionBase = DecisionTypeBase.ConvertFromString(codeString);

            return new DecisionTypeStand
            {
                DecisionBase = decisionBase
            };
        }

        public bool GetDecision(int trueCount, bool softHand, Random random)
        {
            if (DecisionBase.FixedDecision != null)
            {
                bool decision = ((bool)DecisionBase.FixedDecision) ? true : false;
                return decision;
            }

            if (softHand)
            {
                if (trueCount == DecisionBase.IndexNumber)
                {
                    int decision = random.Next(2);
                    return ((decision == 0) ? true : false);
                }
                else
                {
                    return (trueCount > DecisionBase.IndexNumber);
                }
            }
            else
            {
                return (trueCount >= DecisionBase.IndexNumber);
            }            
        }
    }
}
