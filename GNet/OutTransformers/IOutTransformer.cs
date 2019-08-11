namespace GNet
{
    public interface IOutTransformer : ICloneable<IOutTransformer>
    {
        double[] Transform(double[] output);
    }
}
