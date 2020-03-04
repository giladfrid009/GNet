using System;

namespace GNet.Initializers
{
    [Serializable]
    public class Zero : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return 0.0;
        }

        public IInitializer Clone()
        {
            return new Zero();
        }
    }
}