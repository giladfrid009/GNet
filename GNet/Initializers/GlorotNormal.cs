using System;
using static System.Math;
using static GNet.Utils.GRandom;

namespace GNet.Initializers
{
    [Serializable]
    public class GlorotNormal : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return NextNormal() * Sqrt(2.0 / (nIn + nOut));
        }
    }
}