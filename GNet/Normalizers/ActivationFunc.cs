namespace GNet.Normalizers
{
    public class ActivationFunc : INormalizer
    {
        public IActivation Activation { get; }

        public ActivationFunc(IActivation activation)
        {
            Activation = activation;
        }

        public void UpdateParams(ImmutableArray<ImmutableShapedArray<double>> dataVector)
        {
        }

        public ImmutableShapedArray<double> Normalize(ImmutableShapedArray<double> vals)
        {
            return Activation.Activate(vals);
        }
    }
}