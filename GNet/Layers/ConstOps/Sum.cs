using GNet.Model;
using System;

namespace GNet.Layers.ConstOps
{
    [Serializable]
    public class Sum : IConstOp
    {
        public bool RequiresUpdate { get; } = false;

        public ImmutableArray<double> CalcWeights(ImmutableArray<Synapse> inSynapses)
        {
            return inSynapses.Select(X => 1.0);
        }
    }
}