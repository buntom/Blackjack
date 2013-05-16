using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackSim.Configurations.Strategies.Index
{
    public class SoftDoubleDecisionTable : DecisionTableBase<DecisionTypeDouble>
    {
        public SoftDoubleDecisionTable()
            : base(9, 10, "Soft Double Decision Matrix for Index Strategy", DecisionTypeDouble.ConvertFromString)
        {
        }
    }
}
