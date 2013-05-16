using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackSim.Configurations.Strategies.Index
{
    public class HardStandDecisionTable : DecisionTableBase<DecisionTypeStand>
    {
        public HardStandDecisionTable()
            : base(19, 10, "Soft Double Decision Matrix for Index Strategy", DecisionTypeStand.ConvertFromString)
        {
        }
    }
}
