using GNet.Optimizers;

namespace GNet
{
    //todo: test optimizers
    public interface IOptimizer
    {
        IDecay? Decay { get; }

        public void UpdateEpoch(int epoch);

        double Optimize(TrainableObj obj);
    }
}