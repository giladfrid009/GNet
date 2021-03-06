﻿using GNet.Utils;
using System;

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
            return GRandom.Normal(Mean, SD);
        }
    }
}