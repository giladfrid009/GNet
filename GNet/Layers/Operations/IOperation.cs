using GNet.Model;
using NCollections;

namespace GNet.Layers
{
    public interface IOperation
    {
        bool RequiresUpdate { get; }

        NArray<double> CalcWeights(Array<Synapse> inSynapses);
    }
}