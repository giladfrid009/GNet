using System;

namespace GNet.Initializers
{
    [Serializable]
    public class One : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return 1.0;
        }
    }
}