namespace GNet
{
    public interface IInitializer : ICloneable<IInitializer>
    {
        double Initialize(int nIn, int nOut);
    }
}
