using System;
using static System.Math;

namespace GNet.Initializers
{
    [Serializable]
    public class LeCunUniform : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return GRandom.NextDouble(Sqrt(3.0 / nIn));
        }

        public IInitializer Clone()
        {
            return new LeCunUniform();
        }
    }
}