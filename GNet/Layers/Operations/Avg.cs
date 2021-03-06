﻿using GNet.Model;
using System;

namespace GNet.Layers.Operations
{
    [Serializable]
    public class Avg : IOperation
    {
        public bool RequiresUpdate { get; } = false;

        public Array<double> CalcWeights(Array<Synapse> inSynapses)
        {
            int nIn = inSynapses.Length;
            return new Array<double>(nIn, () => 1.0 / nIn);
        }
    }
}