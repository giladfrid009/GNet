using System;
using static GNet.Utils.GRandom;

namespace GNet.Initializers
{
    [Serializable]
    public class Uniform : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return NextDouble(-1.0, 1.0);
        }
    }
}