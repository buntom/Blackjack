using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Serialization;
using BlackjackSim.Configurations;
using BlackjackSim.Results;
using BlackjackSim.Simulation;

namespace BlackjackSim
{
    public class Runner
    {        
        public readonly Configuration Configuration;
        public List<BetHandResult> PlayHandResults { get; private set; }

        public Runner(string configurationPath)
        {
            Configuration = XmlUtils.DeserializeFromFile<Configuration>(configurationPath);
        }

        public void Run()
        {
            var simulation = new Simulation.Simulation(Configuration);
            PlayHandResults = simulation.Run();

            var outFilePath = Configuration.SimulationParameters.OutFilePath;
            PlayHandResults.SaveToFile(outFilePath);
        }

        public double[,] GetResults()
        {
            return PlayHandResults.ConvertToArray();
        }
    }
}
