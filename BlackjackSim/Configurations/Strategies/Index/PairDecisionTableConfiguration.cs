using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BlackjackSim.Configurations.Strategies.Index
{
    public class PairDecisionTableConfiguration
    {
        [XmlArrayItem("Table")]
        public List<PairDecisionTable> PairDecisionTables { get; set; }
    }
}
