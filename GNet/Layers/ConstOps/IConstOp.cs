using GNet.Model;

namespace GNet.Layers
{
    public interface IConstOp
    {
        bool RequiresUpdate { get; }

        ImmutableArray<double> CalcWeights(ImmutableArray<Synapse> inSynapses);
    }
}