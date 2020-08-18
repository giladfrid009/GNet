using GNet.Model;
using NCollections;
using System;

namespace GNet.Layers.Operations
{
    [Serializable]
    public class Sum : IOperation
    {
        public bool RequiresUpdate { get; } = false;

        public NArray<double> CalcWeights(Array<Synapse> inSynapses)
        {
            return new NArray<double>(inSynapses.Length, () => 1);
        }
    }
}