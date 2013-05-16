using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackSim.Configurations.Strategies.Index
{
    public struct DecisionTypeDouble
    {
        public DecisionTypeBase DecisionBase;

        public static DecisionTypeDouble ConvertFromString(string codeString)
        {
            var decisionBase = DecisionTypeBase.ConvertFromString(codeString);

            return new DecisionTypeDouble
            {
                DecisionBase = decisionBase
            };
        }

        public bool GetDecision(int trueCount)
        {
            if (DecisionBase.FixedDecision != null)
            {
                bool decision = ((bool)DecisionBase.FixedDecision) ? true : false;
                return decision;
            }

            if ((bool)DecisionBase.RevertIndexDecision)
            {
                return (trueCount < DecisionBase.IndexNumber);
            }
            else
            {
                return (trueCount >= DecisionBase.IndexNumber);
            }
        }
    }
}
