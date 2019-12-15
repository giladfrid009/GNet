
namespace GNet.Normalizers
{
    public class ActivationFunc : INormalizer
    {
        public IActivation Activation { get; }
        public bool NormalizeInputs { get; set; }
        public bool NormalizeOutputs { get; set; }

        public ActivationFunc(IActivation activation)
        {
            Activation = activation.Clone();
        }

        public void ExtractParams(Dataset dataset) { }

        public ShapedArray<double> Normalize(ShapedReadOnlyArray<double> vals)
        {
            return Activation.Activate(vals);
        }

        public INormalizer Clone()
        {
            return new ActivationFunc(Activation)
            {
                NormalizeInputs = NormalizeInputs,
                NormalizeOutputs = NormalizeOutputs
            };
        }
    }
}
