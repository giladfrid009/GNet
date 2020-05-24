using GNet.Model;
using System;

namespace GNet.Layers.Poolers
{
    [Serializable]
    public class Max : IPooler
    {
        public ImmutableArray<double> CalcWeights(ImmutableArray<Synapse> inSynapses)
        {
            double maxVal = inSynapses.Max(X => X.InNeuron.OutVal);

            return inSynapses.Select(X => X.InNeuron.OutVal == maxVal ? 1.0 : 0.0);
        }
    }
}