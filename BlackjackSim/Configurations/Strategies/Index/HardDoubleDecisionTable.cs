using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackSim.Configurations.Strategies.Index
{
    public class HardDoubleDecisionTable : DecisionTableBase<DecisionTypeDouble>
    {
        public HardDoubleDecisionTable()
            : base(19, 10, "Hard Double Decision Matrix for Index Strategy", DecisionTypeDouble.ConvertFromString)
        {
        }
    }
}
