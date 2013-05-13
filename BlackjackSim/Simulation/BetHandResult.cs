using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackSim.Simulation
{
    public class BetHandResult
    {
        public double Payoff { get; set; }
        public double BetTotal { get; set; }
        public double BetSize { get; set; }
        public int TrueCountBeforeBet { get; set; }
        public int NumberOfSplits { get; set; }

        public static int GetArrayLength()
        {
            return 5;
        }

        public static string GetHeader()
        {
            return "Payoff, NumberOfSplits, TrueCountBeforeBet, BetSize, BetTotal";
        }

        public double[] ConvertToArray()
        {
            return new double[] { Payoff, NumberOfSplits, TrueCountBeforeBet, BetSize, BetTotal };
        }

        public string ConvertToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}",
                Payoff, NumberOfSplits, TrueCountBeforeBet, BetSize, BetTotal);
        }
    }
}