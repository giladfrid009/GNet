namespace GNet.Layers
{
    public interface IPooler
    {
        double Pool(ArrayImmutable<double> vals, out ArrayImmutable<double> inWeights);
    }
}