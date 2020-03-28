namespace GNet.Layers
{
    public interface IPooler
    {
        double Pool(ShapedArrayImmutable<double> vals, out ShapedArrayImmutable<double> inWeights);
    }
}