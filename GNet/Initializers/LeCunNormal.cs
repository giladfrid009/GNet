using System;
using static System.Math;
using static GNet.Utils.GRandom;

namespace GNet.Initializers
{
    [Serializable]
    public class LecunNormal : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return NextNormal() * Sqrt(1.0 / nIn);
        }
    }
}