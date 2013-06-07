using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Configurations;
using BlackjackSim.Serialization;

namespace BlackjackSimSamples.Configurations
{
    public static class ConfigurationSample
    {
        public static void CreateAndSaveSampleConfiguration()
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
                    }
                }
            };

            var fileName = @"B:\sampleConfiguration.xml";

            XmlUtils.SerializeToFile<Configuration>(sampleConfiguration, fileName);
        }
    }
}