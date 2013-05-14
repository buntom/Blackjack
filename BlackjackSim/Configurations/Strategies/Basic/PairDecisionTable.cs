using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using BlackjackSim.Configurations.Strategies;

namespace BlackjackSim.Configurations.Strategies.Basic
{
    public class PairDecisionTable : DecisionTableBase
    {
        [XmlAttribute]
        public bool DoubleAfterSplit { get; set; }

        public PairDecisionTable()
            : base(10, 10, "Pair Decision Matrix for Basic Strategy")
        {
        }
    }
}
