using System;
using static System.Math;
using System.Linq;
using GNet.Extensions; 

namespace GNet.Normalization
{
    public enum Normalizers { None, MinMax, ZScore, DecimalScaling }

    public static class Normalizer
    {
        public static double[] NormalizeVals(double[] vals, Normalizers normMethod)
        {
            switch (normMethod)
            {
                case Normalizers.None: return vals.DeepClone();

                case Normalizers.MinMax: return MinMax(vals);

                case Normalizers.ZScore: return ZScore(vals);

                case Normalizers.DecimalScaling: return DecimalScaling(vals);

                default: throw new ArgumentOutOfRangeException("Unsupported normMethod");
            }
        }

        private static double[] MinMax(double[] vals)
        {
            double[] normalized = new double[vals.Length];

            var min = vals[0];
            var max = vals[0];

            for (int i = 1; i < vals.Length; i++)
            {
                if (vals[i] < min)
                    min = vals[i];

                else if (vals[i] > max)
                    max = vals[i];
            }

            if (min == max)
            {
                for (int i = 0; i < vals.Length; i++)
                {
                    normalized[i] = 0.5;
                }
            }
            else
            {
                for (int i = 0; i < vals.Length; i++)
                {
                    normalized[i] = (vals[i] - min) / (max - min);
                }
            }

            return normalized;
        }

        private static double[] ZScore(double[] vals)
        {
            double[] normalized = new double[vals.Length];

            var mean = vals.Sum() / vals.Length;
            var standardDeviation = Sqrt(vals.Sum(a => (a - mean) * (a - mean)) / vals.Length);

            for (int i = 0; i < vals.Length; i++)
            {
                normalized[i] = (vals[i] - mean) / standardDeviation;
            }

            return normalized;
        }

        private static double[] DecimalScaling(double[] vals)
        {
            double[] normalized = new double[vals.Length];

            var max = Abs(vals[0]);

            for (int i = 1; i < vals.Length; i++)
            {
                if (Abs(vals[i]) > max)
                    max = Abs(vals[i]);
            }

            var scale = (int)Log10(max) + 1;

            for (int i = 0; i < vals.Length; i++)
            {
                normalized[i] = vals[i] / scale;
            }

            return normalized;
        }
    }
}
