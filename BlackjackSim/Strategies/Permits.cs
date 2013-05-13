using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Simulation;
using BlackjackSim.Configurations;

namespace BlackjackSim.Strategies
{
    public class Permits
    {
        public bool Double { get; private set; }
        public bool Split { get; private set; }
        public bool Surrender { get; private set; }

        public Permits(Hand handPlayer, Configuration configuration, int numberOfSplits)
        {
            bool splitDone = numberOfSplits > 0;
            if (splitDone)
            {
                Double = configuration.GameRules.DoubleAfterSplit && handPlayer.CardsHeldCount() == 2;
            }
            else
            {
                Double = handPlayer.CardsHeldCount() == 2;
            }

            Split = numberOfSplits < configuration.GameRules.MaxNumberOfSplits && 
                handPlayer.CardsHeldCount() == 2 && handPlayer.IsPair();

            Surrender = configuration.GameRules.SurrenderAllowed &&
                handPlayer.CardsHeldCount() == 2 && !splitDone;
        }
    }
}
