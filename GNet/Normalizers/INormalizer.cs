namespace GNet
{
    public interface INormalizer : ICloneable<INormalizer>
    {
        bool NormalizeInputs { get; set; }
        bool NormalizeOutputs { get; set; }

        void ExtractParams(Dataset dataset);

        ShapedReadOnlyArray<double> Normalize(ShapedReadOnlyArray<double> vals);
    }
}
