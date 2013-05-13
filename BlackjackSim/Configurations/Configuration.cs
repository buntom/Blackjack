using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackSim.Configurations
{    
    public class Configuration
    {
        public GameRules GameRules { get; set; }
        public SimulationParameters SimulationParameters { get; set; }

        public bool IsValid()
        {
            bool isValid = true;

            if (SimulationParameters.TotalPacksCount <= 0 &&
                (SimulationParameters.BetSizeType == BetSizeType.TRUE_COUNT_VARIABLE ||
                SimulationParameters.StrategyType == StrategyType.INDEX))
            {
                isValid = false;
            }

            // TODO: add some other checks...

            return isValid;
        }
    }
}