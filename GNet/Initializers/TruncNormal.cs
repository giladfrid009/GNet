using GNet.Utils;
using System;

namespace GNet.Initializers
{
    [Serializable]
    public class TruncNormal : IInitializer
    {
        public double Mean { get; }
        public double SD { get; }
        public double Margin { get; }

        public TruncNormal(double mean = 0.0, double sd = 0.05, double margin = 2.0)
        {
            Mean = mean;
            SD = sd;
            Margin = margin;
        }

        public double Initialize(int nIn, int nOut)
        {
            return GRandom.TruncNormal(Mean, SD, Margin);
        }
    }
}