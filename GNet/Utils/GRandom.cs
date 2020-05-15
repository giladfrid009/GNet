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

            double minVal = Exp(-0.5 * Pow((mean - margin) / (sd * Sin(n * Tau)), 2.0));

            return sd * Sqrt(-2.0 * Log(Uniform(minVal, 1.0))) * Sin(Tau * n) + mean;
        }
    }
}