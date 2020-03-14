namespace GNet.Layers
{
    public interface IPooler : ICloneable<IPooler>
    {
        ShapedArrayImmutable<double> GetWeights(ShapedArrayImmutable<double> inValues);
    }
}