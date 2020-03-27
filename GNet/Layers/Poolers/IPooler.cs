namespace GNet.Layers
{
    public interface IPooler
    {
        ShapedArrayImmutable<double> GetWeights(ShapedArrayImmutable<double> inValues);
    }
}