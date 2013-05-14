using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Configurations;
using BlackjackSim.Serialization;
using System.IO;
using BlackjackSim.Configurations.Strategies.Basic;

namespace BlackjackSimSamples.Strategies.Basic
{
    public static class PairDecisionTableConfigurationTest
    {
        public static PairDecisionTableConfiguration LoadConfiguration()
        {
            var filename = Path.Combine("Configuration", "Strategies", "Basic", "PairDecisionTableConfiguration.xml");

            PairDecisionTableConfiguration configuration;

            {
                configuration = XmlUtils.DeserializeFromFile<PairDecisionTableConfiguration>(filename);

                // Here the parsing of "string" matrix is happening
                var matrix = configuration.PairDecisionTables.First().MatrixAsArray;
            }

            {
                //var xmlConfiguration = XmlUtils.DeserializeFromFile<XmlPairDecisionTableConfiguration>(filename);

                // Here the parsing of "string" matrix is happening
                //configuration = PairDecisionTableConfiguration(xmlConfiguration);

                //var matrix = configuration.PairDecisionTables.First().Matrix;
            }

            return configuration;
        }
    }
}
