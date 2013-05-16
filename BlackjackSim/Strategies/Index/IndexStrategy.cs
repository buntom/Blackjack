using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Configurations.Strategies.Index;
using BlackjackSim.Configurations;
using System.IO;
using BlackjackSim.Serialization;
using BlackjackSim.Simulation;

namespace BlackjackSim.Strategies.Index
{
    public class IndexStrategy : IStrategy
    {
        public DecisionTypePair[,] PairDecisionTable { get; private set; }
        public DecisionTypeDouble[,] SoftDoubleDecisionTable { get; private set; }
        public DecisionTypeDouble[,] HardDoubleDecisionTable { get; private set; }
        public DecisionTypeStand[,] SoftStandDecisionTable { get; private set; }
        public DecisionTypeStand[,] HardStandDecisionTable { get; private set; }

        public IndexStrategy(Configuration configuration)
        {
            // load strategy configuration
            var folder = configuration.SimulationParameters.StrategyConfigurationPath;

            var pairDecisionTableConfiguration = Load<PairDecisionTableConfiguration>(folder, "PairDecisionTableConfiguration.xml");
            var doubleDecisionTableConfiguration = Load<DoubleDecisionTableConfiguration>(folder, "DoubleDecisionTableConfiguration.xml");
            var standDecisionTableConfiguration = Load<StandDecisionTableConfiguration>(folder, "StandDecisionTableConfiguration.xml");

            // set decision tables
            var doubleAfterSplit = configuration.GameRules.DoubleAfterSplit;
            var dealerStandsSoft17 = configuration.GameRules.DealerStandsSoft17;

            PairDecisionTable = pairDecisionTableConfiguration.PairDecisionTables
                .Where(table => table.DealerStandsSoft17 == dealerStandsSoft17 && table.DoubleAfterSplit == doubleAfterSplit)
                .Single()
                .MatrixAsArray;

            SoftDoubleDecisionTable = doubleDecisionTableConfiguration.SoftDecisionTables
                .Where(table => table.DealerStandsSoft17 == dealerStandsSoft17)
                .Single()
                .MatrixAsArray;

            HardDoubleDecisionTable = doubleDecisionTableConfiguration.HardDecisionTables
                .Where(table => table.DealerStandsSoft17 == dealerStandsSoft17)
                .Single()
                .MatrixAsArray;

            SoftStandDecisionTable = standDecisionTableConfiguration.SoftDecisionTables
                .Where(table => table.DealerStandsSoft17 == dealerStandsSoft17)
                .Single()
                .MatrixAsArray;

            HardStandDecisionTable = standDecisionTableConfiguration.HardDecisionTables
                .Where(table => table.DealerStandsSoft17 == dealerStandsSoft17)
                .Single()
                .MatrixAsArray;
        }

        private T Load<T>(string folder, string filename) where T : class
        {
            var path = Path.Combine(folder, filename);
            return XmlUtils.DeserializeFromFile<T>(path);
        }

        public bool GetInsuranceDecision(Hand handPlayer, int trueCount)
        {
            return (trueCount >= 3); 
        }

        public StrategyDecisionType GetDecision(Hand handPlayer, Hand handDealer, int trueCount, Permits permits, Random random)
        {
            int playerTotal = handPlayer.Value();

            if (playerTotal >= 21)
            {
                return StrategyDecisionType.STAND;
            }

            int upcardDealer = 0;
            upcardDealer = handDealer.ValueCard(0);

            if (handPlayer.IsPair())
            {
                int cardValue = handPlayer.ValueCard(0);
                var splitDecision = PairDecisionTable[cardValue - 2, upcardDealer - 2];
                if (splitDecision.GetDecision(trueCount) && permits.Split)
                {
                    return StrategyDecisionType.SPLIT;
                }
            }

            bool softHand = handPlayer.IsSoft();
            DecisionTypeDouble doubleDecision;
            if (softHand)
            {
                doubleDecision = SoftDoubleDecisionTable[playerTotal - 12, upcardDealer - 2];
            }
            else
            {
                doubleDecision = HardDoubleDecisionTable[playerTotal - 2, upcardDealer - 2];
            }
            if (doubleDecision.GetDecision(trueCount) && permits.Double)
            {
                return StrategyDecisionType.DOUBLE;
            }

            DecisionTypeStand standDecision;
            if (softHand)
            {
                standDecision = SoftStandDecisionTable[playerTotal - 12, upcardDealer - 2];
            }
            else
            {
                standDecision = HardStandDecisionTable[playerTotal - 2, upcardDealer - 2];
            }
            if (standDecision.GetDecision(trueCount, softHand, random))
            {
                return StrategyDecisionType.STAND;
            }
            else
            {
                return StrategyDecisionType.HIT;
            }
        }
    }
}
