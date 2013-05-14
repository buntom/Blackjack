using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using BlackjackSim.Configurations.Strategies.Basic;

namespace BlackjackSim.Configurations.Strategies.Basic
{
    public class PairDecisionTableConfiguration
    {
        [XmlArrayItem("Table")]
        public List<PairDecisionTable> PairDecisionTables { get; set; }
    }
}
