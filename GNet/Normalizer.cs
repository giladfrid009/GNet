using System;
using static System.Math;
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
            var min = vals[0];
            var max = vals[0];

            vals.ForEach(X => 
            {
                if (X < min) min = X;
                if (X > max) max = X;
            });

            var diff = max - min;

            if (diff == 0)
                return vals.Map(X => 0.5);
                            
            return vals.Map(X => (X - min) / diff);
        }

        private static double[] ZScore(double[] vals)
        {
            double[] normalized = new double[vals.Length];

            var mean = vals.Sum() / vals.Length;
            var standardDeviation = Sqrt(vals.Accumulate(1, (R, X) => R + (X - mean) * (X - mean)) / vals.Length);

            return vals.Map(X => (X - mean) / standardDeviation);
        }

        private static double[] DecimalScaling(double[] vals)
        {
            var max = Abs(vals[0]);

            vals.ForEach(X => 
            {
                if (Abs(X) > max) max = Abs(X);
            });

            var scale = (int)Log10(max) + 1;

            return vals.Map(X => X / scale);
        }
    }
}
