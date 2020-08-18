using GNet.Model;
using NCollections;
using System;

namespace GNet.Layers.Operations
{
    [Serializable]
    public class Max : IOperation
    {
        public bool RequiresUpdate { get; } = true;

        public Array<double> CalcWeights(Array<Synapse> inSynapses)
        {
            double maxVal = inSynapses.Max(X => X.InNeuron.OutVal);

            return inSynapses.Select(X => X.InNeuron.OutVal == maxVal ? 1.0 : 0.0);
        }
    }
}