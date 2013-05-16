using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BlackjackSim.Configurations.Strategies.Index
{
    public class PairDecisionTable : DecisionTableBase<DecisionTypePair>
    {
        [XmlAttribute]
        public bool DoubleAfterSplit { get; set; }

        public PairDecisionTable()
            : base(10, 10, "Pair Decision Matrix for Index Strategy", DecisionTypePair.ConvertFromString)
        {
        }
    }
}
