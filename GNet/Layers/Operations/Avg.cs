using GNet.Model;
using NCollections;
using System;

namespace GNet.Layers.Operations
{
    [Serializable]
    public class Avg : IOperation
    {
        public bool RequiresUpdate { get; } = false;

        public NArray<double> CalcWeights(Array<Synapse> inSynapses)
        {
            int nIn = inSynapses.Length;
            return new NArray<double>(nIn, () => 1.0 / nIn);
        }
    }
}