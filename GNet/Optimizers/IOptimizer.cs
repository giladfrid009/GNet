namespace GNet
{
    public interface IOptimizer
    {
        void Optimize(ILayer layer, int epoch);
    }
}