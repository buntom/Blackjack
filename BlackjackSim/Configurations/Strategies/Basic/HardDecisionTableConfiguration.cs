using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BlackjackSim.Configurations.Strategies.Basic
{
    public class HardDecisionTableConfiguration
    {
        [XmlArrayItem("Table")]
        public List<HardDecisionTable> HardDecisionTables { get; set; }
    }
}