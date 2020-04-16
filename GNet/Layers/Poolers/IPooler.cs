namespace GNet.Layers
{
    public interface IPooler
    {
        double Pool(ImmutableArray<double> vals, out ImmutableArray<double> inWeights);
    }
}