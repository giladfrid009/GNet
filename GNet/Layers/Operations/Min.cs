﻿using GNet.Model;
using NCollections;
using System;

namespace GNet.Layers.Operations
{
    [Serializable]
    public class Min : IOperation
    {
        public bool RequiresUpdate { get; } = true;

        public NArray<double> CalcWeights(Array<Synapse> inSynapses)
        {
            double minVal = inSynapses.Min(X => X.InNeuron.OutVal);

            return inSynapses.Select(X => X.InNeuron.OutVal == minVal ? 1.0 : 0.0).ToNArray();
        }
    }
}