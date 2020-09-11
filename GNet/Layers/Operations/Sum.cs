using GNet.Model;
using System;

namespace GNet.Layers.Operations
{
    [Serializable]
    public class Sum : IOperation
    {
        public bool RequiresUpdate { get; } = false;

        public Array<double> CalcWeights(Array<Synapse> inSynapses)
        {
            return new Array<double>(inSynapses.Length, () => 1);
        }
    }
}