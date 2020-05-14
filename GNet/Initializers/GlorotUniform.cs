using GNet.Utils;
using System;
using static System.Math;

namespace GNet.Initializers
{
    [Serializable]
    public class GlorotUniform : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return GRandom.Uniform(0.0, Sqrt(6.0 / (nIn + nOut)));
        }
    }
}