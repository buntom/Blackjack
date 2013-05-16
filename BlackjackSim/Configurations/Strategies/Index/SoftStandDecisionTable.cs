using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackSim.Configurations.Strategies.Index
{
    public class SoftStandDecisionTable : DecisionTableBase<DecisionTypeStand>
    {
        public SoftStandDecisionTable()
            : base(9, 10, "Soft Stand Decision Matrix for Index Strategy", DecisionTypeStand.ConvertFromString)
        {
        }
    }
}
