using System;

namespace GNet.GlobalRandom
{
    public static class GRandom
    {
        private static readonly Random rnd = new Random();

        public static int Next() => rnd.Next();

        public static int Next(int maxValue) => rnd.Next(maxValue);

        public static int Next(int minValue, int maxValue) => rnd.Next(minValue, maxValue);

        public static double NextDouble() => rnd.NextDouble();

        public static double NextDouble(double minValue, double maxValue) => rnd.NextDouble() * (maxValue - minValue) + minValue;

        public static double NextDouble(double range) => NextDouble(-range, range);

        public static double NextNormal() => Math.Sqrt(-2.0 * Math.Log(1.0 - rnd.NextDouble())) * Math.Sin(2.0 * Math.PI * (1.0 - rnd.NextDouble()));
    }
}
