namespace GNet
{
    public interface INormalizer : ICloneable<INormalizer>
    {
        bool NormalizeInputs { get; set; }
        bool NormalizeOutputs { get; set; }

        void ExtractParams(Dataset dataset);

        ShapedArrayImmutable<double> Normalize(ShapedArrayImmutable<double> vals);
    }
}
