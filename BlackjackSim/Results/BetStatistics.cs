﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Simulation;
using System.IO;
using Diagnostics.Logging;

namespace BlackjackSim.Results
{
    public class BetStatistics
    {
        public double TotalBet { get; private set; }
        public double TotalPal { get; private set; }
        public double TotalInitialBet { get; private set; }
        public double AverageInitialBet { get; private set; }
        public double AverageTotalBet { get; private set; }
        public double MeanPal { get; private set; }
        public double StdPal { get; private set; }
        public double InitialBetAdvantage { get; private set; }
        public double MeanIba { get; private set; }        
        public double StdIba { get; private set; }        
        public double TotalBetAdvantage { get; private set; }
        public double MeanTba { get; private set; }
        public double StdTba { get; private set; }        
        public long NumberOfBets { get; private set; }

        private double SumQuadIba { get; set; }
        private double SumQuadTba { get; set; }
        private double SumQuadPal { get; set; }
                
        public void Update(BetHandResult betHandResult)
        {
            TotalInitialBet += betHandResult.BetSize;
            TotalBet += betHandResult.BetTotal;
            TotalPal += betHandResult.Payoff;            
            NumberOfBets++;
            var iba = betHandResult.Payoff / betHandResult.BetSize;
            var tba = betHandResult.Payoff / betHandResult.BetTotal;
            MeanIba = (MeanIba * (NumberOfBets - 1) + iba) / (double)NumberOfBets;
            MeanTba = (MeanTba * (NumberOfBets - 1) + tba) / (double)NumberOfBets;
                        
            SumQuadIba += Math.Pow(iba, 2);
            SumQuadTba += Math.Pow(tba, 2);
            SumQuadPal += Math.Pow(betHandResult.Payoff, 2);
        }

        public void Complete()
        {
            MeanPal = TotalPal / (double)NumberOfBets;
            AverageInitialBet = TotalInitialBet / (double)NumberOfBets;
            AverageTotalBet = TotalBet / (double)NumberOfBets;
            InitialBetAdvantage = TotalPal / TotalInitialBet;
            TotalBetAdvantage = TotalPal / TotalBet;

            if (NumberOfBets > 1)
            {
                StdPal = Math.Sqrt((double)NumberOfBets / (double)(NumberOfBets - 1) *
                    (1.0 / (double)NumberOfBets * SumQuadPal - Math.Pow(MeanPal, 2)));
                StdIba = Math.Sqrt((double)NumberOfBets / (double)(NumberOfBets - 1) *
                    (1.0 / (double)NumberOfBets * SumQuadIba - Math.Pow(MeanIba, 2)));
                StdTba = Math.Sqrt((double)NumberOfBets / (double)(NumberOfBets - 1) *
                    (1.0 / (double)NumberOfBets * SumQuadTba - Math.Pow(MeanTba, 2)));
            }
        }

        public void WriteToFile(StreamWriter writer)
        {
            Complete();

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
                line = String.Format("Average Initial Bet = {0}", AverageInitialBet);
                writer.WriteLine(line);
                line = String.Format("Average Total Bet = {0}", AverageTotalBet);
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
                line = String.Format("STD TBA = {0}", StdTba);
                writer.WriteLine(line);
            }
            catch (Exception ex)
            {
                TraceWrapper.LogException(ex, "Cannot write bet stats to a file!");
            }
        }
    }
}
