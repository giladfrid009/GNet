using GNet.Model;
using System;

namespace GNet.Layers.Poolers
{
    [Serializable]
    public class Avg : IPooler
    {
        public ImmutableArray<double> CalcWeights(ImmutableArray<Synapse> inSynapses)
        {
            int nIn = inSynapses.Length;

            return inSynapses.Select(X => 1.0 / nIn);
        }
    }
}