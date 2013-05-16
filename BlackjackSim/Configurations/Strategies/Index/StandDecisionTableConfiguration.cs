using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BlackjackSim.Configurations.Strategies.Index
{
    public class StandDecisionTableConfiguration
    {
        [XmlArrayItem("Table")]
        public List<SoftStandDecisionTable> SoftDecisionTables { get; set; }

        [XmlArrayItem("Table")]
        public List<HardStandDecisionTable> HardDecisionTables { get; set; }
    }
}
