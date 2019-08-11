namespace GNet
{
    public interface IOptimizer : ICloneable<IOptimizer>
    {
        void Optimize(Dense layer, int epoch);
    }
}
