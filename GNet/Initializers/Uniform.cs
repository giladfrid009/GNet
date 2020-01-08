using System;
using GNet.GlobalRandom;

namespace GNet.Initializers
{
    [Serializable]
    public class Uniform : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return GRandom.NextDouble(-1.0, 1.0);
        }

        public IInitializer Clone()
        {
            return new Uniform();
        }
    }
}
