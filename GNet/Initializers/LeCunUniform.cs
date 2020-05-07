using System;
using static System.Math;
using static GNet.Utils.GRandom;

namespace GNet.Initializers
{
    [Serializable]
    public class LecunUniform : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return NextDouble(Sqrt(3.0 / nIn));
        }
    }
}