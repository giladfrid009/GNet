using GNet.Model;

namespace GNet.Layers
{
    public interface IPooler
    {
        ImmutableArray<double> CalcWeights(ImmutableArray<Synapse> inSynapses);
    }
}