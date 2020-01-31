using System;

namespace GNet.GlobalRandom
{
    [Serializable]
    public static class GRandom
    {
        private static Random rnd = new Random();

        public static void SetSeed(int seed)
        {
            rnd = new Random(seed);
        }

        public static int Next()
        {
            return rnd.Next();
        }

        public static int Next(int maxValue)
        {
            return rnd.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return rnd.Next(minValue, maxValue);
        }

        public static double NextDouble()
        {
            return rnd.NextDouble();
        }

        public static double NextDouble(double minValue, double maxValue)
        {
            return rnd.NextDouble() * (maxValue - minValue) + minValue;
        }

        public static double NextDouble(double range)
        {
            return NextDouble(-range, range);
        }

        public static double NextNormal()
        {
            return Math.Sqrt(-2.0 * Math.Log(1.0 - rnd.NextDouble())) * Math.Sin(2.0 * Math.PI * (1.0 - rnd.NextDouble()));
        }
    }
}
