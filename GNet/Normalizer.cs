using System.Linq;
using static System.Math;

namespace GNet
{
    public static class Normalizer
    {
        public static double MinMax(double val, double minVal, double maxVal)
        {
            return (val - minVal) / (maxVal - minVal);
        }

        public static double[] MinMax(double[] vals, (double minVal, double maxVal)[] minMaxVals)
        {
            double[] normalized = new double[vals.Length];

            for (int i = 0; i < vals.Length; i++)
            {
                normalized[i] = (vals[i] - minMaxVals[i].minVal) / (minMaxVals[i].maxVal - minMaxVals[i].minVal);
            }

            return normalized;
        }

        public static double[] Zscore(double[] vals)
        {
            double[] normalized = new double[vals.Length];

            var mean = vals.Sum() / vals.Length;
            var standartDeviation = Sqrt(vals.Sum(a => (a - mean) * (a - mean)) / vals.Length);

            for (int i = 0; i < vals.Length; i++)
            {
                normalized[i] = (vals[i] - mean) / standartDeviation;
            }

            return normalized;
        }
    }
}
