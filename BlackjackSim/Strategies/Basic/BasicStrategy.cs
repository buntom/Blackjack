using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Configurations;
using BlackjackSim.Simulation;
using System.IO;
using BlackjackSim.Serialization;
using BlackjackSim.Configurations.Strategies.Basic;

namespace BlackjackSim.Strategies.Basic
{
    public class BasicStrategy : IStrategy
    {
        public DecisionTypePair[,] PairDecisionTable { get; private set; }
        public DecisionType[,] SoftDecisionTable { get; private set; }
        public DecisionType[,] HardDecisionTable { get; private set; }

        public BasicStrategy(Configuration configuration)
        {
            // load strategy configuration
            var folder = configuration.SimulationParameters.StrategyConfigurationPath;

            var pairDecisionTableConfiguration = Load<PairDecisionTableConfiguration>(folder, "PairDecisionTableConfiguration.xml");
            var softDecisionTableConfiguration = Load<SoftDecisionTableConfiguration>(folder, "SoftDecisionTableConfiguration.xml");
            var hardDecisionTableConfiguration = Load<HardDecisionTableConfiguration>(folder, "HardDecisionTableConfiguration.xml");

            // set decision tables
            var doubleAfterSplit = configuration.GameRules.DoubleAfterSplit;
            var dealerStandsSoft17 = configuration.GameRules.DealerStandsSoft17;

            PairDecisionTable = pairDecisionTableConfiguration.PairDecisionTables
                .Where(table => table.DealerStandsSoft17 == dealerStandsSoft17 && table.DoubleAfterSplit == doubleAfterSplit)
                .Single()
                .MatrixAsArray;

            SoftDecisionTable = softDecisionTableConfiguration.SoftDecisionTables
                .Where(table => table.DealerStandsSoft17 == dealerStandsSoft17)
                .Single()
                .MatrixAsArray;

            HardDecisionTable = hardDecisionTableConfiguration.HardDecisionTables
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
            return false; // never take insurance according to the Basic strategy
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

            var strategyDecision = StrategyDecisionType.NA;

            if (handPlayer.IsPair())
            {
                int cardValue = handPlayer.ValueCard(0);
                var pairDecision = PairDecisionTable[cardValue - 2, upcardDealer - 2];
                strategyDecision = DecisionTypePairHelper.ConvertToStrategyDecision(pairDecision, permits);
            }
            if (strategyDecision != StrategyDecisionType.NA)
            {
                return strategyDecision;
            }

            DecisionType decision;
            if (handPlayer.IsSoft())
            {
                decision = SoftDecisionTable[playerTotal - 12, upcardDealer - 2];
            }
            else
            {
                decision = HardDecisionTable[playerTotal - 2, upcardDealer - 2];
            }
            strategyDecision = DecisionTypeHelper.ConvertToStrategyDecision(decision, permits);

            return strategyDecision;
        }
    }
}
