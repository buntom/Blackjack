using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackSim.Configurations
{    
    public class GameRules
    {
        public bool DoubleAfterSplit { get; set; }
        public int MaxNumberOfSplits { get; set; }
        public bool SurrenderAllowed { get; set; }
        public bool DealerStandsSoft17 { get; set; }
        public bool InsuranceAllowed { get; set; }
    }
}
