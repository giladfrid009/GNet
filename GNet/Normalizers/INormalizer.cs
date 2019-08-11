namespace GNet
{
    public interface INormalizer : ICloneable<INormalizer>
    {
        double[] Normalize(double[] vals);
    }
}
