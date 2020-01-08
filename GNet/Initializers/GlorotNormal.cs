using System;
using GNet.GlobalRandom;
using static System.Math;

namespace GNet.Initializers
{
    [Serializable]
    public class GlorotNormal : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return GRandom.NextNormal() * Sqrt(2.0 / (nIn + nOut));
        }

        public IInitializer Clone()
        {
            return new GlorotNormal();
        }
    }
}
