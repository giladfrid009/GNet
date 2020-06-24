using GNet.Optimizers;

namespace GNet
{
    public interface IOptimizer
    {
        IDecay? Decay { get; }

        public void UpdateParams(int epoch);

        double Optimize(TrainableObj obj);
    }
}