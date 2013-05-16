using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BlackjackSim.Configurations.Strategies.Index
{
    public class DoubleDecisionTableConfiguration
    {
        [XmlArrayItem("Table")]
        public List<SoftDoubleDecisionTable> SoftDecisionTables { get; set; }

        [XmlArrayItem("Table")]
        public List<HardDoubleDecisionTable> HardDecisionTables { get; set; }
    }
}
