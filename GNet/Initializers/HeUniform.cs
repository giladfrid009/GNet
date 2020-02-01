using System;
using static System.Math;

namespace GNet.Initializers
{
    [Serializable]
    public class HeUniform : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return GRandom.NextDouble(Sqrt(6.0 / nIn));
        }

        public IInitializer Clone()
        {
            return new HeUniform();
        }
    }
}
