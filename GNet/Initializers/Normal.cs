using System;
using static GNet.Utils.GRandom;

namespace GNet.Initializers
{
    [Serializable]
    public class Normal : IInitializer
    {
        public double Initialize(int nIn, int nOut)
        {
            return NextNormal();
        }
    }
}