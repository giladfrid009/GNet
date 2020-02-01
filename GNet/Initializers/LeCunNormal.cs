using System;
using static System.Math;

namespace GNet.Initializers
{
    [Serializable]
    public class LeCunNormal : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return GRandom.NextNormal() * Sqrt(1.0 / nIn);
        }

        public IInitializer Clone()
        {
            return new LeCunNormal();
        }
    }
}
