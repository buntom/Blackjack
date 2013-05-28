using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace BlackjackSim.Configurations
{
    public class SimulationParameters
    {
        public bool UseCustomSeed { get; set; }
        public int CustomSeed { get; set; }
        public int SimulationCount { get; set; }
        public int TotalPacksCount { get; set; }
        public double PenetrationThreshold { get; set; }
        public double BetSize { get; set; }
        public double BetSizeMin { get; set; }
        public double BetSizeMax { get; set; }
        public BetSizeType BetSizeType { get; set; }
        public string BetSizeTrueCountScaleString { get; set; }
        public string CountSystemString { get; set; }
        public double InitialWealth { get; set; }
        public double RiskAversionCoefficient { get; set; }
        public StrategyType StrategyType { get; set; }
        public string OutputFolder { get; set; }
        public string StrategyConfigurationPath { get; set; }
        public bool SaveFullResults { get; set; }
        public bool SaveAggregatedPal { get; set; }
        public int AggregStatsHandCount { get; set; }

        [XmlIgnore]
        private string outputFolderSpecific;

        [XmlIgnore]
        public string OutputFolderSpecific
        {
            get
            {
                if (outputFolderSpecific == null)
                {
                    var now = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    outputFolderSpecific = Path.Combine(OutputFolder, "BlackjackSimResults_" + now);                    
                }

                return outputFolderSpecific;
            }
        }

        [XmlIgnore]
        private List<CountSystemBit> countSystem;

        [XmlIgnore]
        public List<CountSystemBit> CountSystem
        {
            get
            {
                if (countSystem == null)
                {
                    var items = CountSystemString.Split(';');

                    countSystem = items.Select(item =>
                    {
                        var parts = item.Split(',');

                        Debug.Assert(parts.Length == 2);

                        return new CountSystemBit
                        {
                            Card = int.Parse(parts[0], CultureInfo.InvariantCulture),
                            Count = double.Parse(parts[1], CultureInfo.InvariantCulture)
                        };
                    }).ToList();
                }

                return countSystem;
            }
            set
            {
                countSystem = value;

                var query = countSystem.Select(item => string.Format("{0}, {1}",
                    item.Card.ToString(CultureInfo.InvariantCulture),
                    item.Count.ToString(CultureInfo.InvariantCulture)));

                CountSystemString = string.Join("; ", query);
            }
        }

        [XmlIgnore]
        private List<TrueCountBet> betSizeTrueCountScale;

        [XmlIgnore]
        public List<TrueCountBet> BetSizeTrueCountScale
        {
            get
            {
                if (betSizeTrueCountScale == null)
                {
                    var items = BetSizeTrueCountScaleString.Split(';');

                    betSizeTrueCountScale = items.Select(item =>
                        {
                            var parts = item.Split(',');

                            Debug.Assert(parts.Length == 2);

                            return new TrueCountBet
                            {
                                BetRatio = double.Parse(parts[0], CultureInfo.InvariantCulture),
                                TrueCount = int.Parse(parts[1], CultureInfo.InvariantCulture)
                            };
                        }).ToList();
                }

                return betSizeTrueCountScale;
            }
            set
            {
                betSizeTrueCountScale = value;

                var query = betSizeTrueCountScale.Select(item => string.Format("{0}, {1}",
                    item.BetRatio.ToString(CultureInfo.InvariantCulture),
                    item.TrueCount.ToString(CultureInfo.InvariantCulture)));

                BetSizeTrueCountScaleString = string.Join("; ", query);
            }
        }
    }
}
