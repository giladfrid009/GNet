using GNet.Utils;
using System;
using static System.Math;

namespace GNet.Initializers
{
    [Serializable]
    public class LecunNormal : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return GRandom.Normal() * Sqrt(1.0 / nIn);
        }
    }
}