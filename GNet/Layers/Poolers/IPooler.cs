namespace GNet.Layers
{
    public interface IPooler
    {
        double Pool(ImmutableArray<double> inVals, out ImmutableArray<double> inWeights);
    }
}