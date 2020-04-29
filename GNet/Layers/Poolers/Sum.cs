using GNet.Model;
using System;

namespace GNet.Layers.Poolers
{
    [Serializable]
    public class Sum : IPooler
    {
        public ImmutableArray<double> CalcWeights(ImmutableArray<Synapse> inSynapses)
        {
            return inSynapses.Select(X => 1.0);
        }
    }
}