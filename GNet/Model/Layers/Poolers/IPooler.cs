namespace GNet.Layers
{
    public interface IPooler : ICloneable<IPooler>
    {
        double Pool(ShapedArrayImmutable<double> vals);

        ShapedArrayImmutable<double> Derivative(ShapedArrayImmutable<double> vals);
    }
}
