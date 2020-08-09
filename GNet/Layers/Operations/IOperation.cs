using GNet.Model;

namespace GNet.Layers
{
    public interface IOperation
    {
        bool RequiresUpdate { get; }

        ImmutableArray<double> CalcWeights(ImmutableArray<Synapse> inSynapses);
    }
}