using System;
using static System.Math;

namespace GNet.Initializers
{
    [Serializable]
    public class HeNormal : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return GRandom.NextNormal() * Sqrt(2.0 / nIn);
        }
    }
}