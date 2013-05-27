using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Simulation;
using System.IO;
using Diagnostics.Logging;

namespace BlackjackSim.Results
{
    public class BetStats
    {
        public double TotalBet { get; private set; }
        public double TotalPal { get; private set; }
        public double TotalInitialBet { get; private set; }
        public double MeanPal { get; private set; }
        public double InitialBetAdvantage { get; private set; }
        public double TotalBetAdvantage { get; private set; }
        public double StdPal { get; private set; }
        public double StdIba { get; private set; }        
        public int NumberOfBets { get; private set; }

        private double SumIbaQuad { get; set; }
        private double SumPalQuad { get; set; }
                
        public void Update(BetHandResult betHandResult)
        {
            TotalInitialBet += betHandResult.BetSize;
            TotalBet += betHandResult.BetTotal;
            TotalPal += betHandResult.Payoff;
            
            NumberOfBets++;
            MeanPal = TotalPal / (double)NumberOfBets;
            InitialBetAdvantage = TotalPal / TotalInitialBet;
            TotalBetAdvantage = TotalPal / TotalBet;

            SumIbaQuad += Math.Pow(betHandResult.Payoff / betHandResult.BetSize, 2);
            SumPalQuad += Math.Pow(betHandResult.Payoff, 2);

            if (NumberOfBets > 1)
            {
                StdPal = (double)NumberOfBets / (double)(NumberOfBets - 1) *
                    (1.0 / (double)NumberOfBets * SumPalQuad - Math.Pow(MeanPal, 2));
                StdIba = (double)NumberOfBets / (double)(NumberOfBets - 1) *
                    (1.0 / (double)NumberOfBets * SumIbaQuad - Math.Pow(InitialBetAdvantage, 2));
            }
        }

        public void WriteToFile(StreamWriter writer)
        {
            try
            {
                string line;
                line = String.Format("Nbr of Bets = {0}", NumberOfBets);
                writer.WriteLine(line);
                line = String.Format("Total PaL = {0}", TotalPal);
                writer.WriteLine(line);
                line = String.Format("Total Initial Bet = {0}", TotalInitialBet);
                writer.WriteLine(line);
                line = String.Format("Total Bet = {0}", TotalBet);
                writer.WriteLine(line);
                line = String.Format("Mean PaL = {0}", MeanPal);
                writer.WriteLine(line);
                line = String.Format("STD PaL = {0}", StdPal);
                writer.WriteLine(line);
                line = String.Format("IBA = {0}", InitialBetAdvantage);
                writer.WriteLine(line);
                line = String.Format("STD IBA = {0}", StdIba);
                writer.WriteLine(line);
                line = String.Format("TBA = {0}", TotalBetAdvantage);
                writer.WriteLine(line);
            }
            catch (Exception ex)
            {
                TraceWrapper.LogException(ex, "Cannot write bet stats to a file!");
            }
        }
    }
}
