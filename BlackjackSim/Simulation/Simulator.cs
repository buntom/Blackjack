using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Configurations;
using BlackjackSim.Strategies;
using Diagnostics.Logging;
using System.Diagnostics;
using BlackjackSim.Strategies.Basic;

namespace BlackjackSim.Simulation
{
    public class Simulator
    {
        public readonly Configuration Configuration;
        public IStrategy Strategy;
        public readonly List<TrueCountBet> SortedBetSizeTrueCountScale;
        public Random Random;

        public Simulator(Configuration configuration)
        {
            var simulationParameters = configuration.SimulationParameters;

            Random = simulationParameters.UseCustomSeed ? new Random(simulationParameters.CustomSeed) : new Random();

            Configuration = configuration;
            if (!Configuration.IsValid())
            {
                throw new Exception("Invalid configuration given!");
            }

            switch (simulationParameters.StrategyType)
            {
                case StrategyType.BASIC:
                    Strategy = new BasicStrategy(configuration);
                    break;

                case StrategyType.INDEX:
                    // TODO
                    break;
            }
            
            SortedBetSizeTrueCountScale = simulationParameters.BetSizeTrueCountScale.OrderBy(item => item.TrueCount).ToList();            
        }

        public List<BetHandResult> Run()
        {
            int simulationCount = Configuration.SimulationParameters.SimulationCount;
            var shoe = new CardShoe(Configuration, Random);
            double wealth = Configuration.SimulationParameters.InitialWealth;
            double betSize;
            BetHandResult betHandResult;
            var betHandResultList = new List<BetHandResult>();
            int indexFinished = 0;
            double ratioFinished;

            var stopwatch = Stopwatch.StartNew();            

            for (int i = 0; i < simulationCount; i++)
            {                
                betSize = GetBetSize(wealth, shoe);                
                betHandResult = BetHand(betSize, shoe);

                betHandResultList.Add(betHandResult);
                wealth += betHandResult.Payoff;                

                if (shoe.Penetration > Configuration.SimulationParameters.PenetrationThreshold)
                {
                    shoe.Reinitiate();
                }

                ratioFinished = (double)i / (double)simulationCount * 100.0;
                if ((int)Math.Truncate(ratioFinished / 5.0) > indexFinished)
                {
                    indexFinished++;
                    TraceWrapper.LogInformation("Blackjack simulation: Finished {0}% in {1}.", 
                        ratioFinished, stopwatch.Elapsed);
                }
            }
            stopwatch.Stop();
            TraceWrapper.LogInformation("Blackjack simulation: FINISHED in {0}.", stopwatch.Elapsed);

            return betHandResultList;
        }

        public double GetBetSize(double wealth, CardShoe shoe)
        {
            double betSize = 0;
            switch (Configuration.SimulationParameters.BetSizeType)
            {
                case BetSizeType.FIXED:
                    betSize = Configuration.SimulationParameters.BetSize;
                    break;

                case BetSizeType.TRUE_COUNT_VARIABLE:
                    if (shoe.Count == null)
                    {
                        throw new Exception("Count not initiated, cannot scale bet size conditionally on True Count!");
                    }
                    else
                    {
                        var minTrueCountBet = SortedBetSizeTrueCountScale.First();
                        var maxTrueCountBet = SortedBetSizeTrueCountScale.Last();
                        var trueCount = shoe.Count.TrueCount;

                        var trueCountBet = SortedBetSizeTrueCountScale.Where(item => item.TrueCount == trueCount).FirstOrDefault();
                        if (trueCountBet != null)
                        {
                            betSize = wealth * trueCountBet.BetRatio * (1.0 / Configuration.SimulationParameters.RiskAversionCoefficient);
                        }
                        else if (trueCount < minTrueCountBet.TrueCount)
                        {
                            betSize = wealth * minTrueCountBet.BetRatio * (1.0 / Configuration.SimulationParameters.RiskAversionCoefficient);
                        }
                        else if (trueCount > maxTrueCountBet.TrueCount)
                        {
                            betSize = wealth * maxTrueCountBet.BetRatio * (1.0 / Configuration.SimulationParameters.RiskAversionCoefficient);
                        }

                        betSize = Math.Round(betSize);
                        betSize = Math.Min(Math.Max(betSize, Configuration.SimulationParameters.BetSizeMin),
                            Configuration.SimulationParameters.BetSizeMax);
                        break;
                    }
            }
            
            return betSize;
        }

        public BetHandResult BetHand(double betSize, CardShoe shoe)
        {
            // save the True Count before bet
            int trueCountBeforeBet = 0;
            if (shoe.Count != null)
            {
                trueCountBeforeBet = shoe.Count.TrueCount;
            }

            // initial deal
            Hand handPlayer = new Hand();
            handPlayer.InitialDealPlayer(shoe);
            Hand handDealer = new Hand();
            handDealer.InitialDealDealer(shoe);
            int numberOfSplits = 0;

            // play hand                    
            var betHandResult = new BetHandResult();
            betHandResult.TrueCountBeforeBet = trueCountBeforeBet;            
            betHandResult.BetSize = betSize;
            PlayHandOutcome playHandOutcome = PlayHand(handPlayer, handDealer, betSize, shoe, ref numberOfSplits);
            
            // update bet hand results
            betHandResult.NumberOfSplits = numberOfSplits;
            betHandResult.BetTotal = playHandOutcome.BetTotal;            
            var payoff = PayoffHand(playHandOutcome, handDealer, shoe);
            betHandResult.Payoff = payoff;
            
            // update count with the hole card revealed
            shoe.Count.Update(handDealer.Cards[1], shoe.LeftPacksCount);

            return betHandResult;
        }

        public PlayHandOutcome PlayHand(Hand handPlayer, Hand handDealer, double betSize, CardShoe shoe,
            ref int numberOfSplits)
        {
            int trueCount = 0;
            if (shoe.Count != null)
            {
                trueCount = shoe.Count.TrueCount;
            }
            bool surrenderDone = false;
            bool splitDone = numberOfSplits > 0;            
            var playHandOutcome = new PlayHandOutcome();
            handPlayer.BetSize = betSize;            

            // consider insurance first
            double insuranceBet = 0;
            if (Configuration.GameRules.InsuranceAllowed && handDealer.IsAce(0) && !splitDone)
            {
                bool takeInsurance = Strategy.GetInsuranceDecision(handDealer, trueCount);
                if (takeInsurance)
                {
                    insuranceBet = 0.5 * betSize;
                }
            }

            // player's hand play
            bool continuePlay = true;
            while (handPlayer.Value() < 21 && !surrenderDone && !handDealer.IsBlackjack() && continuePlay)
            {
                Permits permits = new Permits(handPlayer, Configuration, numberOfSplits);
                if (shoe.Count != null)
                {
                    trueCount = shoe.Count.TrueCount;
                }
                StrategyDecisionType decision = Strategy.GetDecision(handPlayer, handDealer, trueCount, permits);

                switch (decision)
                {
                    case StrategyDecisionType.STAND:
                        continuePlay = false;
                        break;

                    case StrategyDecisionType.HIT:
                        handPlayer.Hit(shoe);
                        break;

                    case StrategyDecisionType.DOUBLE:
                        handPlayer.DoubleDone = true;
                        betSize *= 2;
                        handPlayer.Hit(shoe);
                        continuePlay = false;
                        break;

                    case StrategyDecisionType.SPLIT:
                        var splitHands = handPlayer.Split(shoe);                        
                        numberOfSplits++;
                                                
                        var playHandOutcomeSplit1 = PlayHand(splitHands[1], handDealer, betSize, shoe, ref numberOfSplits);
                        var playHandOutcomeSplit2 = PlayHand(splitHands[0], handDealer, betSize, shoe, ref numberOfSplits);
                                                
                        playHandOutcome.AddHands(playHandOutcomeSplit1.HandsPlayed);
                        playHandOutcome.AddHands(playHandOutcomeSplit2.HandsPlayed);                        

                        double betTotal = playHandOutcomeSplit1.BetTotal + playHandOutcomeSplit2.BetTotal + 
                            insuranceBet;
                                                
                        playHandOutcome.BetTotal = betTotal;
                        playHandOutcome.InsuranceBet = insuranceBet;                        
                                                
                        return playHandOutcome;

                    case StrategyDecisionType.SURRENDER:
                        surrenderDone = true;
                        break;

                    case StrategyDecisionType.NA:
                        throw new Exception("Strategy decision was not determined!");
                }
            }
            
            // return outcome            
            playHandOutcome.HandsPlayed.Add(handPlayer);
            playHandOutcome.BetTotal = betSize + insuranceBet;
            playHandOutcome.InsuranceBet = insuranceBet;
            playHandOutcome.SurrenderDone = surrenderDone;

            return playHandOutcome;
        }

        public static double PayoffInsurance(Hand handDealer, double insuranceBet)
        {
            if (handDealer.IsBlackjack())
            {
                return 2.0 * insuranceBet;
            }
            else
            {
                return -insuranceBet;
            }
        }

        public double PayoffHand(PlayHandOutcome playHandOutcome, Hand handDealer, CardShoe shoe)
        {
            var insurancePayoff = PayoffInsurance(handDealer, playHandOutcome.InsuranceBet);
            
            int handsTotal = playHandOutcome.HandsPlayed.Count;
            bool splitDone = handsTotal > 1;
            if (playHandOutcome.SurrenderDone)
            {
                if (handsTotal > 1)
                {
                    throw new Exception("Surrender done while hand has been split - should not happen!");
                }
                var betSize = playHandOutcome.HandsPlayed[0].BetSize;
                return -0.5 * betSize + insurancePayoff;
            }

            // dealer's hand play
            while (((handDealer.Value() <= 16) || (handDealer.Value() == 17 &&
                handDealer.IsSoft() && !Configuration.GameRules.DealerStandsSoft17)) && 
                !playHandOutcome.AllHandBust())
            {
                handDealer.Hit(shoe);
            }
            int dealerTotal = handDealer.Value();

            double payoff = 0;
            foreach (var handPlayed in playHandOutcome.HandsPlayed)
            {
                var betSize = handPlayed.BetSize;
                if (handPlayed.DoubleDone)
                {
                    betSize *= 2;
                }
                int playerTotal = handPlayed.Value();
                
                if (handPlayed.IsBlackjack() && !handDealer.IsBlackjack() && !splitDone)
                {
                    // blackjack won
                    payoff += 1.5 * betSize;
                }
                else if (playerTotal > 21)
                {
                    // player bust
                    payoff -= betSize;
                }
                else if (dealerTotal > 21)
                {
                    // dealer bust
                    payoff += betSize;
                }
                else if (playerTotal > dealerTotal)
                {
                    // player won
                    payoff += betSize;
                }
                else if (dealerTotal > playerTotal)
                {
                    // dealer won
                    payoff -= betSize;
                }
                else
                {
                    // push
                    payoff += 0;
                }
            }

            return payoff + insurancePayoff;
        }
    }
}
