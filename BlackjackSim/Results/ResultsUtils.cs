using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackjackSim.Simulation;
using System.IO;
using Diagnostics.Logging;

namespace BlackjackSim.Results
{
    public static class ResultsUtils
    {
        public static void SaveToFile(this List<BetHandResult> playHandResults, string path)
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    writer.WriteLine(BetHandResult.GetHeader());

                    foreach (var playHandResult in playHandResults)
                    {
                        writer.WriteLine(playHandResult.ConvertToString());
                    }
                }
            }
            catch (Exception ex)
            {
                TraceWrapper.LogException(ex, "Cannot save results to a txt file!");
            }
        }

        public static double[,] ConvertToArray(this List<BetHandResult> playHandResults)
        {
            int m = playHandResults.Count;
            int n = BetHandResult.GetArrayLength();

            var array = new double[m, n];

            for (int i = 0; i < m; i++)
            {
                var resultArray = playHandResults[i].ConvertToArray();

                for (int j = 0; j < n; j++)
                {
                    array[i, j] = resultArray[j];
                }
            }

            return array;
        }
    }
}
