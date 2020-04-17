namespace GNet.Normalizers
{
    public class ActivationFunc : INormalizer
    {
        public IActivation Activation { get; }

        public ActivationFunc(IActivation activation)
        {
            Activation = activation;
        }

        public void UpdateParams<TData>(ImmutableArray<TData> dataVector) where TData : ImmutableArray<double>
        {
        }

        public ImmutableArray<double> Normalize(ImmutableArray<double> vals)
        {
            return Activation.Activate(vals);
        }
    }
}