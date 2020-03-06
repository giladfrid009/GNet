namespace GNet.Layers
{
    public interface IKernel : ICloneable<IKernel>
    {
        bool IsTrainable { get; }

        ShapedArrayImmutable<double> InitWeights(ShapedArrayImmutable<double> inValues);
    }
}