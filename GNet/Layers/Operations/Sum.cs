using GNet.Model;
using System;

namespace GNet.Layers.Operations
{
    [Serializable]
    public class Sum : IOperation
    {
        public bool RequiresUpdate { get; } = false;

        public ImmutableArray<double> CalcWeights(in ImmutableArray<Synapse> inSynapses)
        {
            return inSynapses.Select(X => 1.0);
        }
    }
}