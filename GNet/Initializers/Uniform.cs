using System;
using static GNet.Utils.GRandom;

namespace GNet.Initializers
{
    [Serializable]
    public class Uniform : IInitializer
    {
        public double Min { get; }
        public double Max { get; }

        public Uniform(double min = -0.05, double max = 0.05)
        {
            Min = min;
            Max = max;
        }

        public double Initialize(int nIn, int nOut)
        {
            return NextDouble(Min, Max);
        }
    }
}