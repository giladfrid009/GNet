using System;

namespace GNet.Initializers
{
    [Serializable]
    public class Normal : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return GRandom.NextNormal();
        }

        public IInitializer Clone()
        {
            return new Normal();
        }
    }
}
