using System;
using static System.Math;

namespace GNet.Utils
{
    public static class GRandom
    {
        private static Random rnd = new Random();

        public static void SetSeed(int seed)
        {
            rnd = new Random(seed);
        }

        public static int Next(int maxValue = int.MaxValue)
        {
            return rnd.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return rnd.Next(minValue, maxValue);
        }

        public static double NextDouble(double maxValue = 1.0)
        {
            return maxValue * rnd.NextDouble();
        }

        public static double NextDouble(double minValue, double maxValue)
        {
            return rnd.NextDouble() * (maxValue - minValue) + minValue;
        }

        public static double NextNormal(double mean = 0.0, double sd = 1.0)
        {
            return sd * Sqrt(-2.0 * Log(1.0 - rnd.NextDouble())) * Sin(2.0 * PI * (1.0 - rnd.NextDouble())) + mean;
        }
    }
}