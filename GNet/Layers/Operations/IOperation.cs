﻿using GNet.Model;
using NCollections;

namespace GNet.Layers
{
    public interface IOperation
    {
        bool RequiresUpdate { get; }

        Array<double> CalcWeights(Array<Synapse> inSynapses);
    }
}