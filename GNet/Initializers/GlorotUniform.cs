using System;
using static System.Math;
using static GNet.Utils.GRandom;

namespace GNet.Initializers
{
    [Serializable]
    public class GlorotUniform : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return NextDouble(0.0, Sqrt(6.0 / (nIn + nOut)));
        }
    }
}