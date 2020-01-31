namespace GNet
{
    public interface IOptimizer : ICloneable<IOptimizer>
    {
        void Optimize(ILayer layer, int epoch);
    }
}
