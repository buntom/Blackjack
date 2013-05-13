using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Configurations;
using BlackjackSim.Simulation;

namespace BlackjackSim.Strategies
{
    public class BasicStrategy : IStrategy
    {
        public int[,] PairDecisionTable { get; private set; }
        public int[,] SoftDecisionTable { get; private set; }
        public int[,] HardDecisionTable { get; private set; }

        public BasicStrategy(Configuration configuration)
        {
            // Constructor defines strategy decisions:
            // - split decision:
            // -1 =  not possible
            // 0  =  do not split
            // 1  =  split
            // 2  =  surrender, if NA, then split
            // - decision:
            // -1 =  not possible
            // 0  =  stand
            // 1  =  hit
            // 2  =  double down, if NA, then hit
            // 3  =  double down, if NA, then stand
            // 4  =  surrender, if NA, then hit
            // 5  =  surrender, if NA, then stand

            if (configuration.GameRules.DealerStandsSoft17)
            {
                if (configuration.GameRules.DoubleAfterSplit)
                {
                    // Dealer shows: 
                    //       2  3  4  5  6  7  8  9  T  A 
                    PairDecisionTable = new int[11, 11] {
                        {1,  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
                        {2,  1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
                        {3,  1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
                        {4,  0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
                        {5,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                        {6,  1, 1, 1, 1, 1, 0, 0, 0, 0, 0},
                        {7,  1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
                        {8,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                        {9,  1, 1, 1, 1, 1, 0, 1, 1, 0, 0},
                        {10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                        {11, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                        };
                }
                else //  double after split NOT allowed
                {
                    // Dealer shows: 
                    //       2  3  4  5  6  7  8  9  T  A 
                    PairDecisionTable = new int[11, 11] {
                        {1,  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
                        {2,  0, 0, 1, 1, 1, 1, 0, 0, 0, 0},
                        {3,  0, 0, 1, 1, 1, 1, 0, 0, 0, 0},
                        {4,  0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
                        {5,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                        {6,  1, 1, 1, 1, 1, 0, 0, 0, 0, 0},
                        {7,  1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
                        {8,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                        {9,  1, 1, 1, 1, 1, 0, 1, 1, 0, 0},
                        {10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                        {11, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                        };
                }

                // Dealer shows: 
                //       2  3  4  5  6  7  8  9  T  A 
                SoftDecisionTable = new int[9, 11] {
                    {1,  1, 1, 0, 0, 0, 1, 1, 1, 1, 1},
                    {2,  1, 1, 1, 1, 2, 1, 1, 1, 1, 1},
                    {3,  1, 1, 1, 2, 2, 1, 1, 1, 1, 1},
                    {4,  1, 1, 2, 2, 2, 1, 1, 1, 1, 1},
                    {5,  1, 1, 2, 2, 2, 1, 1, 1, 1, 1},
                    {6,  1, 2, 2, 2, 2, 1, 1, 1, 1, 1},
                    {7,  0, 3, 3, 3, 3, 0, 0, 1, 1, 1},
                    {8,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {9,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    };

                // Dealer shows: 
                //       2  3  4  5  6  7  8  9  T  A 
                HardDecisionTable = new int[20, 11] {
                    {1,  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
                    {2,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                    {3,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                    {4,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                    {5,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                    {6,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                    {7,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                    {8,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                    {9,  1, 2, 2, 2, 2, 1, 1, 1, 1, 1},
                    {10,  2, 2, 2, 2, 2, 2, 2, 2, 1, 1},
                    {11,  2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
                    {12,  1, 1, 0, 0, 0, 1, 1, 1, 1, 1},
                    {13,  0, 0, 0, 0, 0, 1, 1, 1, 1, 1},
                    {14,  0, 0, 0, 0, 0, 1, 1, 1, 1, 1},
                    {15,  0, 0, 0, 0, 0, 1, 1, 1, 4, 1},
                    {16,  0, 0, 0, 0, 0, 1, 1, 4, 4, 4},
                    {17,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {18,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {19,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {20,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    };
            }
            else // dealer hits soft 17
            {
                if (configuration.GameRules.DoubleAfterSplit)
                {
                    // Dealer shows: 
                    //       2  3  4  5  6  7  8  9  T  A 
                    PairDecisionTable = new int[11, 11] {
                        {1,  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
                        {2,  1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
                        {3,  1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
                        {4,  0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
                        {5,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                        {6,  1, 1, 1, 1, 1, 0, 0, 0, 0, 0},
                        {7,  1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
                        {8,  1, 1, 1, 1, 1, 1, 1, 1, 1, 2},
                        {9,  1, 1, 1, 1, 1, 0, 1, 1, 0, 0},
                        {10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                        {11, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                        };
                }
                else //  double after split NOT allowed
                {
                    // Dealer shows: 
                    //       2  3  4  5  6  7  8  9  T  A 
                    PairDecisionTable = new int[11, 11] {
                        {1,  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
                        {2,  0, 0, 1, 1, 1, 1, 0, 0, 0, 0},
                        {3,  0, 0, 1, 1, 1, 1, 0, 0, 0, 0},
                        {4,  0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
                        {5,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                        {6,  1, 1, 1, 1, 1, 0, 0, 0, 0, 0},
                        {7,  1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
                        {8,  1, 1, 1, 1, 1, 1, 1, 1, 1, 2},
                        {9,  1, 1, 1, 1, 1, 0, 1, 1, 0, 0},
                        {10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                        {11, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                        };
                }

                // Dealer shows: 
                //       2  3  4  5  6  7  8  9  T  A 
                SoftDecisionTable = new int[9, 11] {
                    {1,  1, 1, 0, 0, 0, 1, 1, 1, 1, 1},
                    {2,  1, 1, 1, 1, 2, 1, 1, 1, 1, 1},
                    {3,  1, 1, 1, 2, 2, 1, 1, 1, 1, 1},
                    {4,  1, 1, 2, 2, 2, 1, 1, 1, 1, 1},
                    {5,  1, 1, 2, 2, 2, 1, 1, 1, 1, 1},
                    {6,  1, 2, 2, 2, 2, 1, 1, 1, 1, 1},
                    {7,  3, 3, 3, 3, 3, 0, 0, 1, 1, 1},
                    {8,  0, 0, 0, 0, 3, 0, 0, 0, 0, 0},
                    {9,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    };

                // Dealer shows: 
                //       2  3  4  5  6  7  8  9  T  A 
                HardDecisionTable = new int[20, 11] {
                    {1,  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
                    {2,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                    {3,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                    {4,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                    {5,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                    {6,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                    {7,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                    {8,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                    {9,  1, 2, 2, 2, 2, 1, 1, 1, 1, 1},
                    {10,  2, 2, 2, 2, 2, 2, 2, 2, 1, 1},
                    {11,  2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
                    {12,  1, 1, 0, 0, 0, 1, 1, 1, 1, 1},
                    {13,  0, 0, 0, 0, 0, 1, 1, 1, 1, 1},
                    {14,  0, 0, 0, 0, 0, 1, 1, 1, 1, 1},
                    {15,  0, 0, 0, 0, 0, 1, 1, 1, 4, 4},
                    {16,  0, 0, 0, 0, 0, 1, 1, 4, 4, 4},
                    {17,  0, 0, 0, 0, 0, 0, 0, 0, 0, 5},
                    {18,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {19,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {20,  0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    };
            }
        }

        public bool GetInsuranceDecision(Hand handPlayer, int trueCount)
        {
            return false; // never take insurance according to the Basic strategy
        }

        public StrategyDecisionType GetDecision(Hand handPlayer, Hand handDealer, int trueCount, Permits permits)
        {
            int upcardDealer = handDealer.ValueCard(0);
            int playerTotal = handPlayer.Value();

            if (playerTotal >= 21)
            {
                return StrategyDecisionType.STAND;
            }

            int split = 0;
            if (handPlayer.IsPair())
            {
                int cardValue = handPlayer.ValueCard(0);
                split = PairDecisionTable[cardValue - 1, upcardDealer - 1];
            }
            if ((split == 2) && permits.Surrender)
            {
                return StrategyDecisionType.SURRENDER;
            }
            else if ((split == 2) && permits.Surrender)
            {
                split = 1;
            }
            if ((split == 1) && permits.Split)
            {
                return StrategyDecisionType.SPLIT;
            }

            int decision;
            if (handPlayer.IsSoft())
            {
                decision = SoftDecisionTable[playerTotal - 12, upcardDealer - 1];
            }
            else
            {
                decision = HardDecisionTable[playerTotal - 1, upcardDealer - 1];
            }

            if (decision == 0)
            {
                return StrategyDecisionType.STAND;
            }
            if (decision == 1)
            {
                return StrategyDecisionType.HIT;
            }
            if ((decision == 2) && !permits.Double)
            {
                return StrategyDecisionType.HIT;
            }
            if ((decision == 2) && permits.Double)
            {
                return StrategyDecisionType.DOUBLE;
            }
            if ((decision == 3) && !permits.Double)
            {
                return StrategyDecisionType.STAND;
            }
            if ((decision == 3) && permits.Double)
            {
                return StrategyDecisionType.DOUBLE;
            }
            if ((decision == 4) && !permits.Surrender)
            {
                return StrategyDecisionType.HIT;
            }
            if ((decision == 4) && permits.Surrender)
            {
                return StrategyDecisionType.SURRENDER;
            }
            if ((decision == 5) && !permits.Surrender)
            {
                return StrategyDecisionType.STAND;
            }
            if ((decision == 5) && permits.Surrender)
            {
                return StrategyDecisionType.SURRENDER;
            }

            // this should be impossible to reach
            return StrategyDecisionType.NA;
        }
    }
}
