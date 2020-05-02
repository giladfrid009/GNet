namespace GNet.Normalizers
{
    public class ActivationFunc : INormalizer
    {
        public IActivation Activation { get; }

        public ActivationFunc(IActivation activation)
        {
            Activation = activation;
        }

        public void UpdateParams(Dataset dataset, bool inputs, bool targets)
        {
        }

        public double Normalize(double X)
        {
            return Activation.Activate(X);
        }        
    }
}