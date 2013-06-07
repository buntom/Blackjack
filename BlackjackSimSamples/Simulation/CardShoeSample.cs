using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Configurations;
using BlackjackSim.Simulation;

namespace BlackjackSimSamples.Simulation
{
    public class CardShoeSample
    {
        public static void CreateShoe()
        {
            var sampleConfiguration = new Configuration
            {
                GameRules = new GameRules
                {
                    DoubleAfterSplit = true,
                    MaxNumberOfSplits = 10
                },

                SimulationParameters = new SimulationParameters
                {
                    BetSizeBase = 5,
                    TotalPacksCount = 4,
                    InitialWealth = 10000,
                    BetSizeTrueCountScale = new List<TrueCountBet>
                    {
                       new TrueCountBet
                       {
                           BetRatio = 0.2,
                           TrueCount = 10
                       },
                       new TrueCountBet
                       {
                           BetRatio = 0.1,
                           TrueCount = 20
                       },
                       new TrueCountBet
                       {
                           BetRatio = 0.3,
                           TrueCount = 30
                       }
                    },
                    CountSystem = new List<CountSystemBit>
                    {
                       new CountSystemBit
                       {
                           Card = 1,
                           Count = -1
                       },
                       new CountSystemBit
                       {
                           Card = 2,
                           Count = 1
                       },
                       new CountSystemBit
                       {
                           Card = 3,
                           Count = 1
                       },
                       new CountSystemBit
                       {
                           Card = 4,
                           Count = 1
                       },
                       new CountSystemBit
                       {
                           Card = 5,
                           Count = 1
                       },
                       new CountSystemBit
                       {
                           Card = 6,
                           Count = 1
                       },
                       new CountSystemBit
                       {
                           Card = 7,
                           Count = 0
                       },
                       new CountSystemBit
                       {
                           Card = 8,
                           Count = 0
                       },
                       new CountSystemBit
                       {
                           Card = 9,
                           Count = 0
                       },
                       new CountSystemBit
                       {
                           Card = 10,
                           Count = -1
                       },
                       new CountSystemBit
                       {
                           Card = 11,
                           Count = 1
                       },
                       new CountSystemBit
                       {
                           Card = 12,
                           Count = 1
                       },
                       new CountSystemBit
                       {
                           Card = 13,
                           Count = 1
                       }
                    }
                }                
            };

            CardShoe shoe = new CardShoe(sampleConfiguration, new Random());
            var cards = shoe.DealCard(2);
            
        }
    }
}
