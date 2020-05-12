using System;
using static GNet.Utils.GRandom;

namespace GNet.Initializers
{
    [Serializable]
    public class Uniform : IInitializer
    {
        public double MinVal { get; }
        public double MaxVal { get; }

        public Uniform(double minVal = -0.05, double maxVal = 0.05)
        {
            MinVal = minVal;
            MaxVal = maxVal;
        }

        public double Initialize(int nIn, int nOut)
        {
            return NextDouble(MinVal, MaxVal);
        }
    }
}