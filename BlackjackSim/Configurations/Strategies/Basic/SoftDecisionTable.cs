using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BlackjackSim.Configurations.Strategies.Basic
{
    public class SoftDecisionTable : DecisionTableBase
    {
        public SoftDecisionTable()
            : base(9, 10, "Soft Decision Matrix for Basic Strategy")
        {
        }
    }
}
