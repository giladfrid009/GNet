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

        public static int Next(int minValue, int maxValue)
        {
            return rnd.Next(minValue, maxValue);
        }

        public static double Uniform()
        {
            return rnd.NextDouble();
        }

        public static double Uniform(double minValue, double maxValue)
        {
            return rnd.NextDouble() * (maxValue - minValue) + minValue;
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

            double n = rnd.NextDouble();

            double mVal = Exp((mean - margin) * (margin - mean) / (2.0 * sd * sd * Sin(n * Tau) * Sin(n * Tau)));

            return sd * Sqrt(-2.0 * Log(Uniform(mVal, 1.0))) * Sin(Tau * n) + mean;
        }
    }
}