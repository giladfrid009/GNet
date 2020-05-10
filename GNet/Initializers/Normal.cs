using System;
using static GNet.Utils.GRandom;

namespace GNet.Initializers
{
    [Serializable]
    public class Normal : IInitializer
    {
        public double Mean { get; }
        public double SD { get; }

        public Normal(double mean = 0.0, double sd = 0.05)
        {
            Mean = mean;
            SD = sd;
        }

        public double Initialize(int nIn, int nOut)
        {
            return NextNormal(Mean, SD);
        }
    }
}