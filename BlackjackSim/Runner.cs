using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Serialization;
using BlackjackSim.Configurations;
using BlackjackSim.Results;
using BlackjackSim.Simulation;
using BlackjackSim.External;

namespace BlackjackSim
{
    public class Runner
    {        
        public readonly Configuration Configuration;
        public List<BetHandResult> BetHandResults { get; private set; }

        public Runner(string configurationPath)
        {
            Configuration = XmlUtils.DeserializeFromFile<Configuration>(configurationPath);
        }

        public void Run(BlackjackSim.Simulation.Simulator.ProgressBarSetValue progressBarSetValue = null)
        {
            var simulator = new Simulation.Simulator(Configuration);
            simulator.Run(progressBarSetValue);
        }
    }
}
