using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackSim.Simulation
{
    public class PlayHandOutcome
    {
        public List<Hand> HandsPlayed { get; set; }
        public double BetTotal { get; set; }
        public double InsuranceBet { get; set; }
        public bool SurrenderDone { get; set; }

        public PlayHandOutcome()
        {
            HandsPlayed = new List<Hand>();
            BetTotal = 0;
            InsuranceBet = 0;
            SurrenderDone = false;
        }

        public void AddHands(List<Hand> hands)
        {
            HandsPlayed.AddRange(hands);
        }

        public bool AllHandBust()
        {
            if (HandsPlayed.Count == 0)
            {
                return false;
            }

            bool allBust = true;
            for (int i = 0; i < HandsPlayed.Count; i++)
			{
			    allBust = allBust && HandsPlayed[i].IsBust();
			}

            return allBust;
        }
    }    
}
