namespace GNet
{
    public interface INormalizer : ICloneable<INormalizer>
    {
        bool NormalizeInputs { get; set; }
        bool NormalizeOutputs { get; set; }

        void ExtractParams(Dataset dataset);

        ShapedArray<double> Normalize(ShapedArray<double> vals);
    }
}
