using System;
using static System.Math;

namespace GNet.Utils
{
    public static class GRandom
    {
        private const double Tau = 2.0 * PI;

        private static Random rnd = new Random();

        public static void SetSeed(int seed)
        {
            rnd = new Random(seed);
        }

        public static int Next()
        {
            return rnd.Next();
        }

        public static int Next(int minVal, int maxVal)
        {
            return rnd.Next(minVal, maxVal);
        }

        public static double Uniform()
        {
            return rnd.NextDouble();
        }

        public static double Uniform(double minVal, double maxVal)
        {
            return rnd.NextDouble() * (maxVal - minVal) + minVal;
        }

        public static double Normal()
        {
            return Sqrt(-2.0 * Log(rnd.NextDouble())) * Sin(Tau * rnd.NextDouble());
        }

        public static double Normal(double mean, double sd)
        {
            return sd * Normal() + mean;
        }

        public static double TruncNormal(double mean, double sd, double margin)
        {
            if (margin <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(margin));
            }

            double rndVal = rnd.NextDouble();

            double minVal = Exp(-0.5 * Pow((mean - margin) / (sd * Sin(rndVal * Tau)), 2.0));

            return sd * Sqrt(-2.0 * Log(Uniform(minVal, 1.0))) * Sin(Tau * rndVal) + mean;
        }
    }
}