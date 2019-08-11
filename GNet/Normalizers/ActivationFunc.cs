
namespace GNet.Normalizers
{
    public class ActivationFunc : INormalizer
    {
        public IActivation Activation { get; }

        public ActivationFunc(IActivation activation)
        {
            Activation = activation.Clone();
        }

        public double[] Normalize(double[] vals)
        {
            return Activation.Activate(vals);
        }

        public INormalizer Clone()
        {
            return new ActivationFunc(Activation);
        }
    }
}
