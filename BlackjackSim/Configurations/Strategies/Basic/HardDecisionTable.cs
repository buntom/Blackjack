using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BlackjackSim.Configurations.Strategies.Basic
{
    public class HardDecisionTable : DecisionTableBase<DecisionType>
    {
        public HardDecisionTable()
            : base(19, 10, "Hard Decision Matrix for Basic Strategy", DecisionTypeHelper.ConvertFromString)
        {
        }
    }
}
