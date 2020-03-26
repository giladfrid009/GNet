namespace GNet.Normalizers
{
    public class ActivationFunc : INormalizer
    {
        public IActivation Activation { get; }

        public ActivationFunc(IActivation activation)
        {
            Activation = activation.Clone();
        }

        public void ExtractParams(ArrayImmutable<ShapedArrayImmutable<double>> dataVector)
        {
        }

        public ShapedArrayImmutable<double> Normalize(ShapedArrayImmutable<double> vals)
        {
            return Activation.Activate(vals);
        }

        public INormalizer Clone()
        {
            return new ActivationFunc(Activation);
        }
    }
}